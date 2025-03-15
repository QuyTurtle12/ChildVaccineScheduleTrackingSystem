namespace BusinessLogic.DTOs
{
    public class PackageGetDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? Type { get; set; }
        public Guid AppointmentId { get; set; }
        //Fields just for show ui
        public string? AppointmentName { get; set; }
        public DateTimeOffset? CreatedTime { get; set; }
    }

    public class PackagePostDTO
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string Type { get; set; } = string.Empty;
        public Guid AppointmentId { get; set; }
    }

    public class PackagePutDTO
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string Type { get; set; } = string.Empty;
    }
}
