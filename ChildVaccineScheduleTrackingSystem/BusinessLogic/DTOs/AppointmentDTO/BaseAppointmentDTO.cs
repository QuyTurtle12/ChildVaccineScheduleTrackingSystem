using Data.Base;
using Data.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.AppointmentDTO
{
    public class BaseAppointmentDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        //public int? Status { get; set; } = 0;
        // Use the Enum
        [Display(Name = "Trạng thái")]
        public EnumAppointment Status { get; set; }
        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }
        public Guid UserId { get; set; }
        public DateTimeOffset AppointmentDate { get; set; }
    }

}
