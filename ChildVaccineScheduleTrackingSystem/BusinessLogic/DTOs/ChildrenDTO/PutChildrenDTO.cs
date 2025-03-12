namespace BusinessLogic.DTOs.ChildrenDTO
{
    public class PutChildrenDTO : BaseChildrenDTO
    {
        public string Id { get; set; } = string.Empty;
        public int Status { get; set; }
    }
}
