using Data.Base;

namespace Data.Entities
{
    public class Appointment : BaseEntity
    {
        public Guid UserId { get; set; }
        public DateTimeOffset AppointmentDate { get; set; }

        // Navigation Property
        public virtual ICollection<Notification>? Notifications { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<Payment>? Payments { get; set; }
        public virtual ICollection<Feedback>? Feedbacks { get; set; }
        public virtual ICollection<Package>? Packages { get; set; }
        public virtual ICollection<AppointmentPackage>? AppointmentPackages { get; set; }
    }
}
