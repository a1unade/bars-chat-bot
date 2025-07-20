using NotifyHub.Domain.Common.Enums;

namespace NotifyHub.Application.Requests.Notification;

/// <summary>
/// Запрос на создание уведомления
/// </summary>
public class CreateNotificationRequest
{
    /// <summary>
    /// ID пользователя
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Название уведомления
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// Описание уведомления
    /// </summary>
    public string Description { get; set; } = "Описание отсутствует";
    
    /// <summary>
    /// Тип уведомления (разовое/периодическое)
    /// </summary>
    public NotificationType Type { get; set; }
    
    /// <summary>
    /// Частота отправки
    /// </summary>
    public NotificationFrequency? Frequency { get; set; }
    
    /// <summary>
    /// Во сколько запланирована отправка
    /// </summary>
    public DateTime ScheduledAt { get; set; }
}