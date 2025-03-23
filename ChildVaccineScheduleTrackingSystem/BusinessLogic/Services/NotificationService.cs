using AutoMapper;
using BusinessLogic.DTOs.NotificationDTO;
using BusinessLogic.Interfaces;
using Data.Entities;
using Data.ExceptionCustom;
using Data.Interface;
using Data.PaggingItem;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IMapper _mapper;
        private readonly IUOW _unitOfWork;
        private readonly ITokenService _tokenService;

        public NotificationService(IMapper mapper, IUOW unitOfWork, ITokenService tokenService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
        }

        public async Task<PaginatedList<GetNotificationDTO>> GetNotifications(int index, int pageSize, string? userIdSearch, string? messageSearch, string? messageCreatorId)
        {
            if (index <= 0 || pageSize <= 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, "BAD_REQUEST", "Please enter a valid index or pageSize!");
            }

            await AutoAnnouceNextDose();

            IQueryable<Notification> query = _unitOfWork.GetRepository<Notification>()
                .Entities
                .Include(n => n.User)
                .Include(n => n.Appointment)
                .Where(n => !n.DeletedTime.HasValue);

            // Search by UserId
            if (!string.IsNullOrWhiteSpace(userIdSearch))
            {
                query = query.Where(n => n.UserId.Equals(Guid.Parse(userIdSearch)));
            }

            // Search by Message
            if (!string.IsNullOrWhiteSpace(messageSearch))
            {
                query = query.Where(n => n.Message!.Contains(messageSearch));
            }

            // Search by sender
            if (!string.IsNullOrWhiteSpace(messageCreatorId))
            {
                query = query.Where(n => n.CreatedBy!.Equals(messageCreatorId!.ToString()));
            }

            // Sort by Created Time
            query = query.OrderByDescending(n => n.CreatedTime);

            PaginatedList<Notification> resultQuery = await _unitOfWork.GetRepository<Notification>().GetPagging(query, index, pageSize);

            var responseItems = resultQuery.Items.Select(item => _mapper.Map<GetNotificationDTO>(item)).ToList();

            return new PaginatedList<GetNotificationDTO>(
                responseItems,
                resultQuery.TotalCount,
                resultQuery.PageNumber,
                resultQuery.PageSize
            );
        }

        private async Task AutoAnnouceNextDose()
        {
            string currentUserId = _tokenService.GetCurrentUserId();

            // Get current user info
            User? user = await _unitOfWork.GetRepository<User>().Entities
                                    .Where(u => u.Id.Equals(currentUserId))
                                    .Include(u => u.Children)!
                                    .ThenInclude(c => c.VaccineRecords)!
                                    .ThenInclude(vr => vr.Vaccine)
                                    .FirstOrDefaultAsync();

            // Check if user exists
            if (user == null) return;

            // Check if user has children
            if (user.Children == null || !user.Children.Any()) return;

            DateTimeOffset today = DateTimeOffset.UtcNow.Date;
            int reminderThresholdDays = 7;

            foreach (Child child in user.Children)
            {
                // Skip if the child doesn't have any vaccine record 
                if (child.VaccineRecords == null || !child.VaccineRecords.Any()) continue;

                // Get all vaccination records for the child
                ICollection<VaccineRecord> vaccinationRecords = child.VaccineRecords
                    .Where(vr => !vr.DeletedTime.HasValue) // Exclude deleted records
                    .OrderBy(vr => vr.NextDoseDue) // Sort by next due date
                    .ToList();

                foreach (VaccineRecord record in vaccinationRecords)
                {
                    if (record.NextDoseDue > today) // Check only future due dates
                    {
                        TimeSpan timeUntilDue = record.NextDoseDue - today;
                        int daysLeft = (int)timeUntilDue.TotalDays;

                        // Check if the next dose is within 7 days
                        if (daysLeft > 0 && daysLeft <= reminderThresholdDays)
                        {
                            // Check if a notification has already been sent today for this VaccineRecord
                            bool notificationExists = await _unitOfWork.GetRepository<Notification>().Entities
                                .AnyAsync(n =>
                                    n.UserId == Guid.Parse(currentUserId) &&
                                    n.Message!.Contains(record.Id.ToString()) && 
                                    n.CreatedTime.Date == today && // Same day
                                    !n.DeletedTime.HasValue);

                            if (notificationExists)
                            {
                                // Skip sending a new notification since one was already sent today
                                continue;
                            }
                            // Create notification message
                            string message = $"{daysLeft} days left for {child.Name}'s next {record.Vaccine?.Name} dose on {record.NextDoseDue:yyyy-MM-dd}. Vaccine Record ID: {record.Id.ToString()}";

                            // Create a fake appointment
                            Guid appointmentId = Guid.NewGuid();
                            Appointment unExistedAppointment = new Appointment()
                            {
                                AppointmentDate = today, // This is a fake data
                                DeletedTime = today,
                                DeletedBy = currentUserId,
                                UserId = Guid.Parse(currentUserId)
                            };

                            unExistedAppointment.Id = appointmentId;
                            unExistedAppointment.Status = 0;
                            
                            // Save fake appoinment
                            await _unitOfWork.GetRepository<Appointment>().InsertAsync(unExistedAppointment);
                            await _unitOfWork.SaveAsync();

                            // Create a new notification
                            Notification notification = new Notification
                            (
                                Guid.Parse(currentUserId),
                                appointmentId,
                                message
                            );

                            // Save the notification to the database
                            await _unitOfWork.GetRepository<Notification>().InsertAsync(notification);
                            await _unitOfWork.SaveAsync();
                        }
                    }
                }
            }
        }

        public async Task<GetNotificationDTO> GetNotificationById(string id)
        {
            IQueryable<Notification> query = _unitOfWork.GetRepository<Notification>()
                .Entities;

            Notification? notification = await query
                .Where(n => n.Id == Guid.Parse(id))
                .FirstOrDefaultAsync();

            if (notification == null || notification.DeletedTime.HasValue)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, "NOT_FOUND", "Notification not found!");
            }

            return _mapper.Map<GetNotificationDTO>(notification);
        }

        public async Task CreateNotification(PostNotificationDTO postNotification)
        {
            // Validate email
            if (string.IsNullOrWhiteSpace(postNotification.email))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, "BAD_REQUEST", "Email must not be left blank!");
            }

            // Check if user exists (assuming User entity is available)
            var user = await _unitOfWork.GetRepository<User>()
                .Entities
                .Where(u => u.Email == postNotification.email && !u.DeletedTime.HasValue)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, "NOT_FOUND", "User not found!");
            }

            string currentUserId = _tokenService.GetCurrentUserId();

            DateTimeOffset today = DateTimeOffset.UtcNow.Date;

            // Create a fake appointment
            Guid appointmentId = Guid.NewGuid();
            Appointment unExistedAppointment = new Appointment()
            {
                AppointmentDate = today, // This is a fake data
                DeletedTime = today,
                DeletedBy = currentUserId,
                UserId = Guid.Parse(currentUserId)
            };

            unExistedAppointment.Id = appointmentId;
            unExistedAppointment.Status = 0;

            // Save a fake appoinment
            await _unitOfWork.GetRepository<Appointment>().InsertAsync(unExistedAppointment);
            await _unitOfWork.SaveAsync();

            // Map DTO to entity
            Notification newNotification = new Notification(
                user.Id,
                appointmentId,
                postNotification.Message
                );

            // Set audit fields
            newNotification.Status = postNotification.Status;
            newNotification.CreatedTime = DateTime.Now;
            newNotification.CreatedBy = currentUserId;
            newNotification.LastUpdatedBy = currentUserId;
       
            await _unitOfWork.GetRepository<Notification>().InsertAsync(newNotification);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateNotification(PutNotificationDTO updatedNotification)
        {
            IQueryable<Notification> query = _unitOfWork.GetRepository<Notification>().Entities;

            Notification? notification = await query
                .Where(n => n.Id == Guid.Parse(updatedNotification.Id))
                .FirstOrDefaultAsync();

            if (notification == null || notification.DeletedTime.HasValue)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, "NOT_FOUND", "Notification not found!");
            }

            // Map DTO to entity
            _mapper.Map(updatedNotification, notification);

            // Update audit fields
            notification.LastUpdatedTime = DateTimeOffset.Now;
            notification.LastUpdatedBy = _tokenService.GetCurrentUserId();

            await _unitOfWork.GetRepository<Notification>().UpdateAsync(notification);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteNotificationById(string id)
        {
            IQueryable<Notification> query = _unitOfWork.GetRepository<Notification>().Entities;

            Notification? notification = await query
                .Where(n => n.Id == Guid.Parse(id))
                .FirstOrDefaultAsync();

            if (notification == null || notification.DeletedTime.HasValue)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, "NOT_FOUND", "Notification not found!");
            }

            // Soft delete
            notification.Status = 0;
            notification.LastUpdatedTime = DateTimeOffset.Now;
            notification.DeletedTime = notification.LastUpdatedTime;

            await _unitOfWork.GetRepository<Notification>().UpdateAsync(notification);
            await _unitOfWork.SaveAsync();
        }
    }
}
