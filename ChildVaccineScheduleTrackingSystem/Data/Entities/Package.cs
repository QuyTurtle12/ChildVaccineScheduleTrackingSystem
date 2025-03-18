using Data.Base;

namespace Data.Entities
{
    public class Package : BaseEntity
    {
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string Type { get; set; } = string.Empty;
        public DateTime? ExecuteDate { get; set; }

        // Navigation Property
        public virtual Appointment? Appointment { get; set; }
        public virtual ICollection<PackageVaccine>? PackageVaccines { get; set; }
        public virtual ICollection<AppointmentPackage>? AppointmentPackages { get; set; }
    }
}
