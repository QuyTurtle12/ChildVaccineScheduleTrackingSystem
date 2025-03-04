using Data.Base;

namespace Data.Entities
{
    public class Feedback : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid AppointmentId { get; set; }
        public int? Rating { get; set; }
        public string? Comment { get; set; }
        // Feedback date is createdTime in base entity

        // Navigation Property
        public virtual Appointment? Appointment { get; set; }
        public virtual User? User { get; set; }
    }
}
