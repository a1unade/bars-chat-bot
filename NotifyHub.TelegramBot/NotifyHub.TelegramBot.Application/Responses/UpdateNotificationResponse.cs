namespace NotifyHub.TelegramBot.Application.Responses;

public class UpdateNotificationResponse
{
    public NotificationResponse UpdateNotification { get; set; } = null!;
}

public class NotificationResponse
{
    public Guid Id { get; set; }
}