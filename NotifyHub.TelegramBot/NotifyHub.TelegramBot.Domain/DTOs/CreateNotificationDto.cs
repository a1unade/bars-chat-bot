using NotifyHub.Abstractions.Enums;

namespace NotifyHub.TelegramBot.Domain.DTOs;

/// <summary>
/// Создание уведомления
/// </summary>
public class CreateNotificationDto
{
    /// <summary>
    /// Заголовок
    /// </summary>
    public string Title { get; set; } = null!;
    
    /// <summary>
    /// Описание 
    /// </summary>
    public string Description { get; set; } = null!;
    
    /// <summary>
    /// Тип (по отправке)
    /// </summary>
    public NotificationType Type { get; set; }
    
    /// <summary>
    /// Когда запланирована отправка
    /// </summary>
    public DateTime ScheduledAt { get; set; }
    
    /// <summary>
    /// Частота отправки
    /// </summary>
    public NotificationFrequency? Frequency { get; set; }
}