using System.Security.Claims;
using AutoMapper;
using BusinessLogic.DTOs.AppointmentDTO;
using BusinessLogic.Interfaces;
using Data.Constants;
using Data.Entities;
using Data.Enum;
using Data.ExceptionCustom;
using Data.Interface;
using Data.PaggingItem;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class AppointmentService : IAppointmentService
    {

        private readonly IMapper _mapper;
        private readonly IUOW _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public AppointmentService(IMapper mapper, IUOW unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }
        private string GetCurrentUserName()
        {
            return _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Unknown";
        }
        private Guid GetCurrentUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId) ? userId : Guid.Empty;
        }

        private string GetCurrentUserRole()
        {
            var roleClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role);
            return roleClaim?.Value ?? "Customer"; // Default to "Customer" if role is missing
        }

        public async Task<IEnumerable<GetAppointmentDTO>> GetAllAppointments()
        {
            IQueryable<Appointment> query = _unitOfWork.GetRepository<Appointment>().Entities;
            query = query.Where(a => !a.DeletedTime.HasValue); // Exclude deleted appointments
            query = query.OrderBy(c => c.Name);
            IEnumerable<Appointment> appointments = await query.ToListAsync();
            return _mapper.Map<IEnumerable<GetAppointmentDTO>>(appointments);
        }

        public async Task<GetAppointmentDTO> GetAppointmentById(Guid id)
        {
            Appointment? appointment = await _unitOfWork.GetRepository<Appointment>().GetByIdAsync(id);
            if (appointment == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Appointment not found!");
            }
            GetAppointmentDTO responseItem = _mapper.Map<GetAppointmentDTO>(appointment);
            return responseItem;
        }

        public async Task<PaginatedList<GetAppointmentDTO>> GetAppointments(int index, int pageSize, Guid? idSearch, string? userSearch, string? userIdSearch, string? nameSearch, DateTimeOffset? fromDateSearch, DateTimeOffset? toDateSearch, int? statusSearch)
        {
            var currentUserId = GetCurrentUserId();
            var currentUserRole = GetCurrentUserRole();

            if (index <= 0 || pageSize <= 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Please input index or page size correctly!");
            }

            IQueryable<Appointment> query = _unitOfWork.GetRepository<Appointment>().Entities.Include(a => a.User);

            // Only show records where DeletedBy is null
            query = query.Where(u => u.DeletedBy == null);

            // Search by Appointment id
            if (idSearch.HasValue)
            {
                query = query.Where(u => u.Id == idSearch);
            }

            // If the user is a customer, filter by their UserId
            if (currentUserRole == "Customer")
            {
                query = query.Where(a => a.UserId == currentUserId);
            }

            // Search by user name
            if (!string.IsNullOrWhiteSpace(userSearch))
            {
                query = query.Where(u => u.User!.Name.Contains(userSearch.Trim()));
            }

            // Search by user id
            if (!string.IsNullOrWhiteSpace(userIdSearch))
            {
                query = query.Where(u => u.UserId.Equals(Guid.Parse(userIdSearch)));
            }

            // Search by name
            if (!string.IsNullOrWhiteSpace(nameSearch))
            {
                query = query.Where(u => u.Name!.Contains(nameSearch.Trim()));
            }



            if (statusSearch.HasValue)
            {
                query = query.Where(u => u.Status == statusSearch);
            }

            // Filter by Date Range (From - To)
            if (fromDateSearch.HasValue)
            {
                query = query.Where(a => a.AppointmentDate >= fromDateSearch);
            }

            if (toDateSearch.HasValue)
            {
                query = query.Where(a => a.AppointmentDate <= toDateSearch);
            }

            // Sort by Appointment Date (Newest First)
            query = query.OrderByDescending(a => a.AppointmentDate);

            PaginatedList<Appointment> resultQuery = await _unitOfWork.GetRepository<Appointment>().GetPagging(query.Include(u => u.User), index, pageSize);

            // Map user entities to user dto
            IReadOnlyCollection<GetAppointmentDTO> responseItems = resultQuery.Items.Select(item =>
            {
               
                GetAppointmentDTO responseItem = new GetAppointmentDTO();
                responseItem.Id = item.Id;
                responseItem.UserId = item.User != null ? item.User.Id : Guid.Empty;
                responseItem.UserName = item.User != null ? item.User.Name : "Unknown";
                responseItem.AppointmentDate = item.AppointmentDate;
                responseItem.Name = item.Name;
                responseItem.Status = item.Status.HasValue ? (EnumAppointment)item.Status.Value : EnumAppointment.Pending;
                responseItem.CreatedBy = item.CreatedBy;
                responseItem.LastUpdatedBy = item.LastUpdatedBy;
                responseItem.DeletedBy = item.DeletedBy;
                responseItem.CreatedTime = item.CreatedTime;
                responseItem.DeletedTime = item.DeletedTime;
                responseItem.LastUpdatedTime = item.LastUpdatedTime;

                return responseItem;
            }).ToList();

            PaginatedList<GetAppointmentDTO> paginatedList = new(
                responseItems,
                resultQuery.TotalCount,
                resultQuery.PageNumber,
                resultQuery.PageSize
                );

            return paginatedList;
        }

        public async Task CreateAppointment(PostAppointmentDTO appointmentDto)
        {
            if (appointmentDto == null)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Apppointment data is required!");
            }

            string currentUser = GetCurrentUserName();

            if(appointmentDto.AppointmentDate <= DateTimeOffset.Now)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Apppointment date can not be a day in the past or today!");
            }

            appointmentDto.Status = 0;
            appointmentDto.CreatedBy = currentUser;
            appointmentDto.CreatedTime = DateTimeOffset.UtcNow;

            Appointment appointment = _mapper.Map<Appointment>(appointmentDto);
            await _unitOfWork.GetRepository<Appointment>().InsertAsync(appointment);
            await _unitOfWork.SaveAsync();

        }

        public async Task UpdateAppointment(PutAppointmentDTO putAppointmentDto)
        {
            IGenericRepository<Appointment> repository = _unitOfWork.GetRepository<Appointment>();
            Appointment? existingAppointment = await repository.GetByIdAsync(putAppointmentDto.Id);
            if (existingAppointment == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.BADREQUEST, "Appointment not found!");
            }
            // Update properties
            string currentUser = GetCurrentUserName();

            putAppointmentDto.LastUpdatedBy = currentUser;
            putAppointmentDto.LastUpdatedTime = DateTimeOffset.Now;

            _mapper.Map(putAppointmentDto, existingAppointment);

            repository.Update(existingAppointment);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAppointment(Guid id)
        {
            IGenericRepository<Appointment> repository = _unitOfWork.GetRepository<Appointment>();
            Appointment? existingAppointment = await repository.GetByIdAsync(id);
            if (existingAppointment == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.BADREQUEST, "Appointment not found!");
            }

            string currentUser = GetCurrentUserName();

            existingAppointment.DeletedBy = currentUser;
            existingAppointment.DeletedTime = DateTime.Now;

            repository.Update(existingAppointment);
            await _unitOfWork.SaveAsync();
        }

        
    }
}
