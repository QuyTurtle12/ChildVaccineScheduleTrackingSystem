namespace Data.Entities
{
    public class PackageVaccine
    {
        public Guid Id { get; set; }
        public Guid PackageId { get; set; }
        public Guid VaccineId { get; set; }

        public PackageVaccine() 
        {
            Id = Guid.NewGuid();
        }

        // Navigation Property
        public virtual Package? Package { get; set; }
        public virtual Vaccine? Vaccine { get; set; }
    }
}
