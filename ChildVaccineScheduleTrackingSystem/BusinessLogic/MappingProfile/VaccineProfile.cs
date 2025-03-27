using AutoMapper;
using BusinessLogic.DTOs;
using Data.Entities;

namespace BusinessLogic.MappingProfile
{
    public class VaccineProfile : Profile
    {
        public VaccineProfile()
        {
            CreateMap<VaccineGetDto, Vaccine > ().ReverseMap();
            CreateMap<VaccinePostDto, Vaccine>().ReverseMap();
            CreateMap<VaccinePutDto, Vaccine>().ReverseMap();
        }
    }
}
