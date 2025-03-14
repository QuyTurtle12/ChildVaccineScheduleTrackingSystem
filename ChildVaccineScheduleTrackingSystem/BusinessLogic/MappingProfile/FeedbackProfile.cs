using AutoMapper;
using BusinessLogic.DTOs.UserDTO;
using BusinessLogic.DTOs.FeedbackDTO;
using Data.Entities;
using BusinessLogic.DTOs.PaymentDTO;

namespace BusinessLogic.Mapping;

public class FeedbackProfile : Profile
{
    public FeedbackProfile()
    {
        /*
         * // User mappings
        CreateMap<User, GetUserDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.RoleName, opt => opt.Ignore()); 
        CreateMap<PostUserDTO, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) 
            .ForMember(dest => dest.RoleId, opt => opt.Ignore()); 

        CreateMap<PutUserDTO, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) 
            .ForMember(dest => dest.RoleId, opt => opt.Ignore()); 

        // Feedback mappings
        CreateMap<Feedback, GetFeedbackDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId.ToString()))
            .ForMember(dest => dest.UserName, opt => opt.Ignore()); 
        CreateMap<PostFeedbackDTO, Feedback>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) 
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => Guid.Parse(src.UserId)));

        CreateMap<PutFeedbackDTO, Feedback>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) 
            .ForMember(dest => dest.UserId, opt => opt.Ignore()); */


        CreateMap<GetFeedbackDTO, Feedback>().ReverseMap();
        CreateMap<PostFeedbackDTO, Feedback>().ReverseMap();
        CreateMap<PutFeedbackDTO, Feedback>().ReverseMap();





    }
}