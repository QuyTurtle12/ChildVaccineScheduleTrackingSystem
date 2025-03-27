using AutoMapper;
using BusinessLogic.DTOs;
using Data.Entities;

namespace BusinessLogic.MappingProfile
{
    public class PackageProfile : Profile
    {
        public PackageProfile()
        {
            CreateMap<PackageGetDTO, Package>().ReverseMap();
            CreateMap<PackagePostDTO, Package>().ReverseMap();
            CreateMap<PackagePutDTO, Package>().ReverseMap();
        }
    }
}
