using AutoMapper;
using BusinessLogic.DTOs.FeedbackDTO;
using BusinessLogic.DTOs.NotificationDTO;
using Data.Entities;

namespace BusinessLogic.Mapping
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            /* CreateMap<Notification, GetNotificationDTO>()
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                 .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId.ToString()));

             CreateMap<PostNotificationDTO, Notification>()
                 .ForMember(dest => dest.Id, opt => opt.Ignore())
                 .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => Guid.Parse(src.UserId)));

             CreateMap<PutNotificationDTO, Notification>()
                 .ForMember(dest => dest.Id, opt => opt.Ignore());*/

            CreateMap<GetNotificationDTO, Notification>().ReverseMap()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.AppointmentDate, opt => opt.MapFrom(src => src.Appointment.AppointmentDate));
            CreateMap<PostNotificationDTO, Notification>().ReverseMap();
            CreateMap<PutNotificationDTO, Notification>().ReverseMap();

        }
    }
}