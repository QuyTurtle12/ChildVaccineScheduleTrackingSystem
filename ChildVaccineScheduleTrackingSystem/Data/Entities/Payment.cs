using Data.Base;

namespace Data.Entities
{
    public class Payment : BaseEntity
    {
        public Guid AppointmentId { get; set; }
        public decimal Amount { get; set; }
        public string? PaymentMethod { get; set; }
        // Payment date is createdTime in base Entity

        // Navigation Property
        public virtual Appointment? Appointment { get; set; }
    }
}
