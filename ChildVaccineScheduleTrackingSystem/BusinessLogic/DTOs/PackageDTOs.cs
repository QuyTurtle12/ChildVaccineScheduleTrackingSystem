namespace BusinessLogic.DTOs
{
    public class PackageGetDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; } = 0;
        public string? Type { get; set; }
        //Fields just for show ui
        public DateTimeOffset? CreatedTime { get; set; }
    }

    public class PackagePostDTO
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; } = 0;
        public string Type { get; set; } = string.Empty;
        //Fields to add package's vaccine
        public List<Guid> SelectedVaccineIds { get; set; } = new();
    }

    public class PackagePutDTO
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string Type { get; set; } = string.Empty;
        //Fields to add/remove package's vaccine
        public List<Guid> SelectedVaccineIds { get; set; } = new();
    }
}
