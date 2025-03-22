namespace BusinessLogic.DTOs
{
    public class VaccineGetDto
    {
        public string Name { get; set; } = string.Empty;
        public Guid Id { get; set; }
        public string? Manufacturer { get; set; }
        public string? Description { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }
        //Fields to show
        public int? Status { get; set; }
        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }
    }

    public class VaccinePostDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Manufacturer { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }

        public string ImageUrl { get; set; }
        

        public DateTimeOffset CreatedTime { get; set; }
        public string? CreatedBy { get; set; }
    }

    public class VaccinePutDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Manufacturer { get; set; }
        public string? Description { get; set; }

        public decimal Price { get; set; }
        public string ImageUrl { get; set; }

        public int? Status { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public string? LastUpdatedBy { get; set; }
    }

}
