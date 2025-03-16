using Data.Base;

namespace Data.Entities
{
    public class Notification : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid AppointmentId { get; set; }
        public string? Message { get; set; }
        // Send date is createdTime in base entity

        // Navigation Property
        public virtual User? User { get; set; }
        public virtual Appointment? Appointment { get; set; }
    }
}
