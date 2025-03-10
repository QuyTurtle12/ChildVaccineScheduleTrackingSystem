using AutoMapper;
using BusinessLogic.DTOs.AppointmentDTO;
using BusinessLogic.DTOs.PaymentDTO;
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
    public class PaymentService : IPaymentService
    {
        private readonly IMapper _mapper;
        private readonly IUOW _unitOfWork;

        public PaymentService(IMapper mapper, IUOW unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<GetPaymentDTO>> GetAllPayments()
        {
            IQueryable<Payment> query = _unitOfWork.GetRepository<Payment>().Entities;
            query = query.OrderBy(c => c.Name);
            IEnumerable<Payment> payments = await query.ToListAsync();
            return _mapper.Map<IEnumerable<GetPaymentDTO>>(payments);
        }

        public async Task<GetPaymentDTO> GetPaymentById(Guid id)
        {
            Payment? payment = await _unitOfWork.GetRepository<Payment>().GetByIdAsync(id);
            if (payment == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Payment not found!");
            }
            GetPaymentDTO responseItem = _mapper.Map<GetPaymentDTO>(payment);
            return responseItem;
        }

        public async Task<PaginatedList<GetPaymentDTO>> GetPayments(int index, int pageSize, Guid? idSearch, string? paymentMethodSearch, decimal? fromAmountSearch, decimal? toAmountSearch, string? nameSearch, int? statusSearch)
        {
            if (index <= 0 || pageSize <= 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Please input index or page size correctly!");
            }

            IQueryable<Payment> query = _unitOfWork.GetRepository<Payment>().Entities;

            // Search by category id
            if (idSearch.HasValue)
            {
                query = query.Where(u => u.Id == idSearch);
            }

            // Search by user
            if (!string.IsNullOrWhiteSpace(paymentMethodSearch))
            {
                query = query.Where(u => u.PaymentMethod!.Contains(paymentMethodSearch));
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

            PaginatedList<Payment> resultQuery = await _unitOfWork.GetRepository<Payment>().GetPagging(query.Include(u => u.Appointment), index, pageSize);

            // Map user entities to user dto
            IReadOnlyCollection<GetPaymentDTO> responseItems = resultQuery.Items.Select(item =>
            {

                GetPaymentDTO responseItem = new GetPaymentDTO();
                responseItem.Id = item.Id;
                responseItem.AppointmentId = item.Appointment != null ? item.Appointment.Id : Guid.Empty;
                responseItem.Amount = item.Amount;
                responseItem.PaymentMethod = item.PaymentMethod;
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

            PaginatedList<GetPaymentDTO> paginatedList = new(
                responseItems,
                resultQuery.TotalCount,
                resultQuery.PageNumber,
                resultQuery.PageSize
                );

            return paginatedList;
        }

        public async Task CreatePayment(PostPaymentDTO paymentDto)
        {
            if (paymentDto == null)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Payment data is required!");
            }
            paymentDto.CreatedBy = "system"; // Will use token
            paymentDto.CreatedTime = DateTime.Now;
            paymentDto.LastUpdatedBy = "system"; // Will use token
            paymentDto.LastUpdatedTime = DateTime.Now;

            Payment payment = _mapper.Map<Payment>(paymentDto);

            await _unitOfWork.GetRepository<Payment>().InsertAsync(payment);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdatePayment(PutPaymentDTO putPaymentDto)
        {
            IGenericRepository<Payment> repository = _unitOfWork.GetRepository<Payment>();
            Payment? existingPayment = await repository.GetByIdAsync(putPaymentDto.Id);
            if (existingPayment == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.BADREQUEST, "Payment not found!");
            }

            // Preserve CreatedBy and CreatedTime
            putPaymentDto.CreatedBy = existingPayment.CreatedBy;
            putPaymentDto.CreatedTime = existingPayment.CreatedTime;
            existingPayment.LastUpdatedBy = "system"; // Will use token
            existingPayment.LastUpdatedTime = DateTime.Now;

            // Update properties
            _mapper.Map(putPaymentDto, existingPayment);


            repository.Update(existingPayment);
            await _unitOfWork.SaveAsync();
        }
        public async Task DeletePayment(Guid id)
        {
            IGenericRepository<Payment> repository = _unitOfWork.GetRepository<Payment>();
            Payment? existingPayment = await repository.GetByIdAsync(id);
            if (existingPayment == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.BADREQUEST, "Payment not found!");
            }

            // Preserve existing values
            existingPayment.CreatedBy = existingPayment.CreatedBy;
            existingPayment.CreatedTime = existingPayment.CreatedTime;
            existingPayment.LastUpdatedBy = existingPayment.LastUpdatedBy;
            existingPayment.LastUpdatedTime = existingPayment.LastUpdatedTime;

            existingPayment.DeletedBy = "system"; // Will use token
            existingPayment.DeletedTime = DateTime.Now;

            repository.Update(existingPayment);
            await _unitOfWork.SaveAsync();
        }
    }
}
