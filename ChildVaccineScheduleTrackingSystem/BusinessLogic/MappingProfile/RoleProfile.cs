using AutoMapper;
using BusinessLogic.DTOs.RoleDTO;
using Data.Entities;

namespace BusinessLogic.MappingProfile
{
    public class RoleProfile : Profile
    {
        public RoleProfile() 
        {
            CreateMap<GetRoleDTO, Role>().ReverseMap();
        }
    }
}
