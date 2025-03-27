using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.NotificationDTO
{
    public class PutNotificationDTO
    {
        public string Id { get; set; }
        public string? Message { get; set; }
        public int Status { get; set; }
    }
}
