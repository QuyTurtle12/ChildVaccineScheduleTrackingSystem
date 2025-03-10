﻿using AutoMapper;
using Azure;
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
                responseItem.UserId = item.User != null ? item.User.Id : Guid.Empty;
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

        public async Task CreateAppointment(PostAppointmentDTO appointmentDto)
        {
            if (appointmentDto == null)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Apppointment data is required!");
            }
            appointmentDto.CreatedBy = "system"; // Will use token
            appointmentDto.CreatedTime = DateTime.Now;
            appointmentDto.LastUpdatedBy = "system"; // Will use token
            appointmentDto.LastUpdatedTime = DateTime.Now;

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

            existingAppointment.DeletedBy = "system"; // Will use token
            existingAppointment.DeletedTime = DateTime.Now;

            repository.Update(existingAppointment);
            await _unitOfWork.SaveAsync();
        }

        
    }
}
