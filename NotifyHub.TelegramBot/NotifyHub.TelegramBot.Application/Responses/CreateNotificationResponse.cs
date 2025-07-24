namespace NotifyHub.TelegramBot.Application.Responses;

/// <summary>
/// Ответ на запрос с созданием уведомления
/// </summary>
public class CreateNotificationResponse
{
    public NotificationIdOnly CreateNotification { get; set; } = null!;
}

public class NotificationIdOnly
{
    /// <summary>
    /// ID созданного уведомления
    /// </summary>
    public Guid Id { get; set; }
}