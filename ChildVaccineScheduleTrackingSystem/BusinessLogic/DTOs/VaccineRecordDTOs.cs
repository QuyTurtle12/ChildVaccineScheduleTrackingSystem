namespace BusinessLogic.DTOs
{
    public class GetVaccineRecordDto
    {
        public Guid Id { get; set; }
        public Guid ChildId { get; set; }
        public Guid VaccineId { get; set; }
        public DateTimeOffset DateAdministered { get; set; }
        public DateTimeOffset NextDoseDue { get; set; }
        public string? CustomerNote { get; set; }
        //Extended fields for user
        public string? ChildName { get; set; }
        public string? VaccineName { get; set; }
    }

    public class PostVaccineRecordDto
    {
        public Guid ChildId { get; set; }
        public Guid VaccineId { get; set; }
        public DateTimeOffset DateAdministered { get; set; }
        public DateTimeOffset NextDoseDue { get; set; }
        public string? CustomerNote { get; set; }
    }

    public class PutVaccineRecordDto
    {
        public DateTimeOffset DateAdministered { get; set; }
        public DateTimeOffset NextDoseDue { get; set; }
        public string? CustomerNote { get; set; }
    }
}
