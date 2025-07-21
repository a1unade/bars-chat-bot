using AutoMapper;
using NotifyHub.Abstractions.DTOs;
using NotifyHub.Domain.Entities;
using NotifyHub.Application.Requests.Notification;

namespace NotifyHub.Application.Mappings;

public class NotificationMappingProfile: Profile
{
    public NotificationMappingProfile()
    {
        CreateMap<CreateNotificationRequest, Notification>();
        
        CreateMap<Notification, NotificationDto>()
            .ForMember(dest => dest.TelegramTag, opt => opt.MapFrom(src => src.User.TelegramTag))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email));
    }
}