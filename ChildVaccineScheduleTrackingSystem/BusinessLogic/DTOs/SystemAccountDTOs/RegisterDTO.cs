using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.DTOs.SystemAccountDTOs
{
    public class RegisterDTO
    {
        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(6)]
        public string Password { get; set; }

        [Required, Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        // Optional: if you want to set a role during registration (default can be "User")
        public Guid? RoleId { get; set; }
    }
}
