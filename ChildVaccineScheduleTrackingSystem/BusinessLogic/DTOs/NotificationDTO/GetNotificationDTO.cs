using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.NotificationDTO
{
    public class GetNotificationDTO
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }   
        public DateTimeOffset AppointmentDate { get; set; }
        public string? Message { get; set; }
        public int Status { get; set; }
        public string? CreatedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public string? LastUpdatedBy { get; set; }
        public DateTimeOffset? LastUpdatedTime { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }
    }
}
