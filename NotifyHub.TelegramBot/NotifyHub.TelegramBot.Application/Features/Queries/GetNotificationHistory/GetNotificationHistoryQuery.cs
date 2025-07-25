using MediatR;
using NotifyHub.TelegramBot.Domain.DTOs;

namespace NotifyHub.TelegramBot.Application.Features.Queries.GetNotificationHistory;

public class GetNotificationHistoryQuery(long telegramUserId): IRequest<List<NotificationHistoryDto>>
{
    public long TelegramUserId { get; } = telegramUserId;
}