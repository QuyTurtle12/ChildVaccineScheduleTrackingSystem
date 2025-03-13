namespace BusinessLogic.DTOs.UserDTO
{
    public class GetUserDTO : BaseUserDTO
    {
        public string Id { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Status { get; set; }
    }
}
