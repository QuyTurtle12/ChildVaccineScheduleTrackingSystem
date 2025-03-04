using Data.Base;

namespace Data.Entities
{
    public class Vaccine : BaseEntity
    {
        public string? Manufacturer { get; set; }
        public string? Description { get; set; }

        // Navigation Property
        public virtual ICollection<PackageVaccine>? PackageVaccines { get; set; }
        public virtual ICollection<VaccineRecord>? VaccineRecords { get; set; } 
    }
}
