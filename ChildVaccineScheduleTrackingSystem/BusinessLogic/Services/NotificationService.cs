using AutoMapper;
using BusinessLogic.DTOs.NotificationDTO;
using BusinessLogic.Interfaces;
using Data.Entities;
using Data.ExceptionCustom;
using Data.Interface;
using Data.PaggingItem;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IMapper _mapper;
        private readonly IUOW _unitOfWork;

        public NotificationService(IMapper mapper, IUOW unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedList<GetNotificationDTO>> GetNotifications(int index, int pageSize, string? emailSearch, string? messageSearch)
        {
            if (index <= 0 || pageSize <= 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, "BAD_REQUEST", "Please enter a valid index or pageSize!");
            }
            

            IQueryable<Notification> query = _unitOfWork.GetRepository<Notification>()
                .Entities
                .Include(n => n.User)
                .Include(n => n.Appointment)
                .Where(n => !n.DeletedTime.HasValue);

            // Search by UserId
            if (!string.IsNullOrWhiteSpace(emailSearch))
            {
                IQueryable<User> uquery = _unitOfWork.GetRepository<User>().Entities;

                User? user = await uquery.Where(u => u.Email == emailSearch).FirstOrDefaultAsync();
                if (user != null) {
                    query = query.Where(n => n.UserId == user.Id); 
                }
                
            }

            // Search by Message
            if (!string.IsNullOrWhiteSpace(messageSearch))
            {
                query = query.Where(n => n.Message.Contains(messageSearch));
            }

            // Sort by Id
            query = query.OrderBy(n => n.Id);

            PaginatedList<Notification> resultQuery = await _unitOfWork.GetRepository<Notification>().GetPagging(query, index, pageSize);

            var responseItems = resultQuery.Items.Select(item => _mapper.Map<GetNotificationDTO>(item)).ToList();

            return new PaginatedList<GetNotificationDTO>(
                responseItems,
                resultQuery.TotalCount,
                resultQuery.PageNumber,
                resultQuery.PageSize
            );
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


            // Map DTO to entity
            Notification newNotification = new Notification(
                user.Id,
                postNotification.AppointmentId,
                postNotification.Message
                );

            // Set audit fields
            newNotification.Status = postNotification.Status;
            newNotification.CreatedTime = DateTime.Now;
       
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
