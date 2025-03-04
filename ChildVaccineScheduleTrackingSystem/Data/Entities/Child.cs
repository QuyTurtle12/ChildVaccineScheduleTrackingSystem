using Data.Base;

namespace Data.Entities
{
    public class Child : BaseEntity
    {
        public Guid UserId { get; set; }
        public DateTimeOffset AppointmentDate { get; set; }
        public int Age { get; set; }

        // Navigation Property
        public virtual User? User { get; set; }
        public virtual ICollection<VaccineRecord>? VaccineRecords { get; set; }
    }
}
