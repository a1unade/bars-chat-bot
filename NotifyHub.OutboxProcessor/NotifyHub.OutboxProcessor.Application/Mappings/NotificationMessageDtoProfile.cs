using AutoMapper;
using NotifyHub.Abstractions.DTOs;

namespace NotifyHub.OutboxProcessor.Application.Mappings;

public class NotificationMessageDtoProfile: Profile
{
    public NotificationMessageDtoProfile()
    {
        CreateMap<NotificationDto, NotificationMessageDto>();
    }
}