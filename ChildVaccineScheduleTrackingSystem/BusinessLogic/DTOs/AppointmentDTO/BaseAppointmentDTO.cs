using Data.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.AppointmentDTO
{
    public class BaseAppointmentDTO : BaseEntity
    {
        public string UserName { get; set; }
        public DateTimeOffset? AppointmentDate { get; set; }
    }
}
