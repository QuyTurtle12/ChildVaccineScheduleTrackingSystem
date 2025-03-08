using AutoMapper;
using BusinessLogic.DTOs.AppointmentDTO;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.MappingProfile
{
    public class AppointmentProfile : Profile
    {
        public AppointmentProfile()
        {
            CreateMap<Appointment, GetAppointmentDTO>().ReverseMap();
            CreateMap<Appointment, PostAppointmentDTO>().ReverseMap();
            CreateMap<Appointment, PutAppointmentDTO>().ReverseMap();
        }
    }
}
