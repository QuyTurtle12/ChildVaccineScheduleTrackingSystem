using Data.Enum;

namespace BusinessLogic.DTOs.UserDTO
{
    public class PostUserDTO : BaseUserDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmedPassword { get; set; } = string.Empty;
        public EnumRole RoleName { get; set; }

    }
}
