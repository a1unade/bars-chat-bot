using NotifyHub.TelegramBot.Domain.DTOs;

namespace NotifyHub.TelegramBot.Application.Responses;

public class NotificationListResponse
{
    public List<UserNotificationDto> Notifications { get; set; } = new();
}