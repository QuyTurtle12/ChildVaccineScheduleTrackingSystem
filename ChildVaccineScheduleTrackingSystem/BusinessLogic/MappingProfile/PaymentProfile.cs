using AutoMapper;
using BusinessLogic.DTOs.PaymentDTO;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.MappingProfile
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<Payment, GetPaymentDTO>().ReverseMap();
            CreateMap<Payment, PostPaymentDTO>().ReverseMap();
            CreateMap<Payment, PutPaymentDTO>().ReverseMap();
        }
    }
}
