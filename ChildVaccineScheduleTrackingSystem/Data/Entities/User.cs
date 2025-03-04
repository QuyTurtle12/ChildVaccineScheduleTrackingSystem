using Data.Base;

namespace Data.Entities;

public class User : BaseEntity
{

    public string? Email { get; set; }

    public Guid RoleId { get; set; }

    public string Password { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }
    
    public string? Address { get; set; }

    // Navigation Property
    public virtual ICollection<Child>? Children { get; set; }
    public virtual Role? Role { get; set; }
    public virtual ICollection<Notification>? Notifications { get; set; }
    public virtual ICollection<Appointment>? Appointments { get; set; }
    public virtual ICollection<Feedback>? Feedbacks { get; set; }
}
