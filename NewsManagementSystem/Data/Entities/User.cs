using Data.Base;

namespace Data.Entities;

public partial class User : BaseEntity
{

    public string? Email { get; set; }

    public Guid RoleID { get; set; }

    public string Password { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }
    
    public string? Address { get; set; }

}
