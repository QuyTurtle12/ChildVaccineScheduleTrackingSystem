using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.NotificationDTO
{
    public class PostNotificationDTO
    {
        public string email { get; set; }

        public Guid AppointmentId { get; set; }
        public string? Message { get; set; }
        public int Status { get; set; } = 1; // Default active
    }
}
