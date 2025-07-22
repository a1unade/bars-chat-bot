using AutoMapper;
using NotifyHub.Abstractions.DTOs;
using NotifyHub.NotificationService.Domain.Common.Enums;
using NotifyHub.NotificationService.Domain.Entities;

namespace NotifyHub.NotificationService.Application.Mappings;

public class NotificationLogProfile: Profile
{
    public NotificationLogProfile()
    {
        CreateMap<NotificationMessageDto, NotificationLog>()
            .ForMember(dest => dest.NotificationId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.TelegramId, opt => opt.MapFrom(src => src.TelegramUserId))
            .ForMember(dest => dest.SentAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => OperationStatus.InProgress))
            .ForMember(dest => dest.ErrorMessage, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}