using NotifyHub.Domain.Common.Enums;

namespace NotifyHub.Application.Requests.Notification;

/// <summary>
/// Запрос на обновление уведомления по ID
/// </summary>
public class UpdateNotificationRequest
{
    /// <summary>
    /// ID уведомления
    /// </summary>
    public required Guid Id { get; set; }
    
    /// <summary>
    /// Название уведомления
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Описание уведомления
    /// </summary>
    public string? Description { get; set; } = "Описание отсутствует";
    
    /// <summary>
    /// Тип уведомления (разовое/периодическое)
    /// </summary>
    public NotificationType? Type { get; set; }
    
    /// <summary>
    /// Частота отправки
    /// </summary>
    public NotificationFrequency? Frequency { get; set; }
    
    /// <summary>
    /// Во сколько запланирована отправка
    /// </summary>
    public DateTime? ScheduledAt { get; set; }
}