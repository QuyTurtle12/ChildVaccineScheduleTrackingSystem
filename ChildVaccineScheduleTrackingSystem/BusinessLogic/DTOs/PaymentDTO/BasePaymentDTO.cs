using Data.Base;
using Data.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.PaymentDTO
{
    public class BasePaymentDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public EnumPayment Status { get; set; }
        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }
        public Guid AppointmentId { get; set; }
        public decimal Amount { get; set; }
        public string? PaymentMethod { get; set; }
    }
}
