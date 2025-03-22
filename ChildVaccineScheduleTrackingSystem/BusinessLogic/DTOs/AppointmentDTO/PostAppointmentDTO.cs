using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.AppointmentDTO
{
    public class PostAppointmentDTO : BaseAppointmentDTO
    {
        public string? CustomerPhoneNumber { get; set; }
        public string? PaymentName { get; set; }
        //public Guid? PackageId { get; set; }
        public List<Guid> PackageIds { get; set; } = new List<Guid>(); // Allow multiple packages

    }
}
