using System.Text.Json;
using AutoMapper;
using NotifyHub.Abstractions.DTOs;
using NotifyHub.OutboxProcessor.Domain.Common.Enums;
using NotifyHub.OutboxProcessor.Domain.Entities;

namespace NotifyHub.OutboxProcessor.Application.Mappings;

public class OutboxMessageProfile: Profile
{
    public OutboxMessageProfile()
    {
        CreateMap<NotificationDto, OutboxMessage>()
            .ForMember(dest => dest.NotificationId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
            .ForMember(dest => dest.Frequency, opt => opt.MapFrom(src => src.Frequency))
            .ForMember(dest => dest.ScheduledAt, opt => opt.MapFrom(src => src.ScheduledAt))
            .ForMember(dest => dest.PayloadJson, opt => opt.MapFrom(src => Serialize(src)))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => OperationStatus.Created))
            .ForMember(dest => dest.SentAt, opt => opt.Ignore())
            .ForMember(dest => dest.Error, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
    }

    private static string Serialize(NotificationDto dto)
        => JsonSerializer.Serialize(dto, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
}