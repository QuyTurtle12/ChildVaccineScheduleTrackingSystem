﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.AppointmentDTO
{
    public class GetAppointmentDTO : BaseAppointmentDTO
    {
        public string UserName { get; set; } = string.Empty;
    }
}
