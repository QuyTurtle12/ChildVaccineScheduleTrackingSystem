using BusinessLogic.DTOs.PaymentDTO;
using Data.PaggingItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IPaymentService
    {
        Task<IEnumerable<GetPaymentDTO>> GetAllPayments();
        Task<GetPaymentDTO> GetPaymentById(Guid id);
        Task<PaginatedList<GetPaymentDTO>> GetPayments(int index, int pageSize, Guid? idSearch, string? paymentMethodSearch, decimal? fromAmountSearch, decimal? toAmountSearch, string? nameSearch, int? statusSearch);

        Task CreatePayment(PostPaymentDTO paymentDto);
        Task UpdatePayment(PutPaymentDTO paymentDto);
        Task DeletePayment(Guid id);
        Task DeletePaymentByAppointmentId(Guid appointmentId);
    }
}
