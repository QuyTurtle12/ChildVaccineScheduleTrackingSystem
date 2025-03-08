using AutoMapper;
using BusinessLogic.DTOs.AppointmentDTO;
using BusinessLogic.Interfaces;
using Data.Constants;
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
    public class AppointmentService : IAppointmentService
    {

        private readonly IMapper _mapper;
        private readonly IUOW _unitOfWork;

        public AppointmentService(IMapper mapper, IUOW unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<GetAppointmentDTO>> GetAllAppointments()
        {
            IQueryable<Appointment> query = _unitOfWork.GetRepository<Appointment>().Entities;
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

        public async Task<PaginatedList<GetAppointmentDTO>> GetAppointments(int index, int pageSize, Guid? idSearch, string? userSearch, string? nameSearch, DateTimeOffset? fromDateSearch, DateTimeOffset? toDateSearch, int? statusSearch)
        {
            if (index <= 0 || pageSize <= 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Please input index or page size correctly!");
            }

            IQueryable<Appointment> query = _unitOfWork.GetRepository<Appointment>().Entities;

            // Search by category id
            if (idSearch.HasValue)
            {
                query = query.Where(u => u.Id == idSearch);
            }

            // Search by user
            if (!string.IsNullOrWhiteSpace(userSearch))
            {
                query = query.Where(u => u.User!.Name.Contains(userSearch));
            }

            // Search by name
            if (!string.IsNullOrWhiteSpace(nameSearch))
            {
                query = query.Where(u => u.Name!.Contains(nameSearch));
            }


            if (statusSearch.HasValue)
            {
                query = query.Where(u => u.Status == statusSearch);
            }

            // Sort by Id
            query = query.OrderBy(u => u.Id);

            PaginatedList<Appointment> resultQuery = await _unitOfWork.GetRepository<Appointment>().GetPagging(query.Include(u => u.User), index, pageSize);

            // Map user entities to user dto
            IReadOnlyCollection<GetAppointmentDTO> responseItems = resultQuery.Items.Select(item =>
            {
               
                GetAppointmentDTO responseItem = new GetAppointmentDTO();
                responseItem.Id = item.Id;
                responseItem.UserName = item.User != null ? item.User.Name : "Unknown";
                responseItem.AppointmentDate = item.AppointmentDate;
                responseItem.Name = item.Name;
                responseItem.Status = item.Status;
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

        public async Task<PostAppointmentDTO> CreateAppointment(PostAppointmentDTO appointmentDto)
        {
            if (appointmentDto == null)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Apppointment data is required!");
            }

            Appointment appointment = _mapper.Map<Appointment>(appointmentDto);

            await _unitOfWork.GetRepository<Appointment>().InsertAsync(appointment);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<PostAppointmentDTO>(appointment);
        }

        public async Task<PutAppointmentDTO> UpdateAppointment(Guid id, PutAppointmentDTO appointmentDto)
        {
            if (appointmentDto == null || id == Guid.Empty)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Invalid appointment data or id!");
            }

            
            Appointment appointment = await _unitOfWork.GetRepository<Appointment>().GetByIdAsync(id);

            if (appointment == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Appointment not found!");
            }

            // Map the updated fields to the category entity
            _mapper.Map(appointmentDto, appointment);

            // Update the category in the database
            _unitOfWork.GetRepository<Appointment>().Update(appointment);
            await _unitOfWork.SaveAsync();

            // Return the updated category
            return _mapper.Map<PutAppointmentDTO>(appointment);
        }

        public async Task<bool> DeleteAppointment(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Invalid appointment ID!");
            }

            // Find the category by id
            Appointment appointment = await _unitOfWork.GetRepository<Appointment>().GetByIdAsync(id);

            if (appointment == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Appointment not found!");
            }

            // Remove the category from the database
            await _unitOfWork.GetRepository<Appointment>().DeleteAsync(appointment);
            await _unitOfWork.SaveAsync();

            return true;
        }

        
    }
}
