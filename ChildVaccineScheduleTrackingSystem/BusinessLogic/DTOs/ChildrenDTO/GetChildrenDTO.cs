namespace BusinessLogic.DTOs.ChildrenDTO
{
    public class GetChildrenDTO : BaseChildrenDTO
    {
        public string Id { get; set; } = string.Empty;
        public string ParentName { get; set; } = string.Empty;
        public string ParentEmail { get; set; } = string.Empty;
        public int Status { get; set; }
    }
}
