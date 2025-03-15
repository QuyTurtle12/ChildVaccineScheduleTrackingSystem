using Data.Base;

namespace Data.Entities
{
    public class VaccineRecord : BaseEntity
    {
        public Guid ChildId { get; set; }
        public Guid VaccineId { get; set; }
        public DateTimeOffset DateAdministered { get; set; }
        public DateTimeOffset NextDoseDue { get; set; }

        // Navigation Property
        public virtual Vaccine? Vaccine { get; set; }
        public virtual Child? Child { get; set; }

    }
}
