using Data.Base;

namespace Data.Entities
{
    public class Role : BaseEntity
    {
        // Navigation Property
        public virtual ICollection<User>? Users { get; set; }
    }
}
