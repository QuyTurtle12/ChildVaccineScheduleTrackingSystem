namespace Data.Entities
{
    public class AppointmentPackage
    {
        public Guid Id { get; set; }
        public Guid PackageId { get; set; }
        public Guid AppointmentId { get; set; }

        public AppointmentPackage()
        {
            Id = Guid.NewGuid();
        }

        // Navigation Property
        public virtual Package? Package { get; set; }
        public virtual Appointment? Appointment { get; set; }
    }
}
