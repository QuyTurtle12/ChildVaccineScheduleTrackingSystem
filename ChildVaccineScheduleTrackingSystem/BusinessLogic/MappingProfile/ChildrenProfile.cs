using AutoMapper;
using BusinessLogic.DTOs.ChildrenDTO;
using Data.Entities;

namespace BusinessLogic.MappingProfile
{
    public class ChildrenProfile : Profile
    {
        public ChildrenProfile() 
        {
            CreateMap<GetChildrenDTO, Child>().ReverseMap();
            CreateMap<PostChildrenDTO, Child>().ReverseMap();
            CreateMap<PutChildrenDTO, Child>().ReverseMap();
            CreateMap<PutChildrenDTO, GetChildrenDTO>().ReverseMap();
        }
    }
}
