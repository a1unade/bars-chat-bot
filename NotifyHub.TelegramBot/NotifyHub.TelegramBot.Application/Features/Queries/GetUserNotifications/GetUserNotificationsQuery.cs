using MediatR;
using NotifyHub.TelegramBot.Domain.DTOs;

namespace NotifyHub.TelegramBot.Application.Features.Queries.GetUserNotifications;

public class GetUserNotificationsQuery: IRequest<List<UserNotificationDto>>
{
    /// <summary>
    /// Id пользователя в Telegram
    /// </summary>
    public long TelegramUserId { get; set; }
}