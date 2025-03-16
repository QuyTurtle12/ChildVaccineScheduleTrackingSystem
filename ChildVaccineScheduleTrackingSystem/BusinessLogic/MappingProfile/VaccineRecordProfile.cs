using AutoMapper;
using BusinessLogic.DTOs;
using Data.Entities;

namespace BusinessLogic.MappingProfile
{
    public class VaccineRecordProfile : Profile
    {
        public VaccineRecordProfile()
        {
            CreateMap<GetVaccineRecordDto, VaccineRecord>().ReverseMap();
            CreateMap<PostVaccineRecordDto, VaccineRecord>().ReverseMap();
            CreateMap<PutVaccineRecordDto, VaccineRecord>().ReverseMap();
        }
    }
}
