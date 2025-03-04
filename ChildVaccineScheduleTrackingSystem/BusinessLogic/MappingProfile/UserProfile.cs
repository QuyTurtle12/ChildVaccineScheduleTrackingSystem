using AutoMapper;
using BusinessLogic.DTOs.UserDTO;
using Data.Entities;

namespace BusinessLogic.MappingProfile
{
    public class UserProfile : Profile
    {
        public UserProfile() 
        {
            CreateMap<GetUserDTO, User>().ReverseMap();
            CreateMap<PostUserDTO, User>().ReverseMap();
            CreateMap<PutUserDTO, User>().ReverseMap();
        }
    }
}
