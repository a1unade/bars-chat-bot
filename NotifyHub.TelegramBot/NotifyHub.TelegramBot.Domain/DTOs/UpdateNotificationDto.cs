using NotifyHub.Abstractions.Enums;

namespace NotifyHub.TelegramBot.Domain.DTOs;

public class UpdateNotificationDto
{
    /// <summary>
    /// ID уведомления
    /// </summary>
    public Guid? Id { get; set; }
    
    /// <summary>
    /// Новое название для уведомления
    /// </summary>
    public string? Title { get; set; }
    
    /// <summary>
    /// Новое описание
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// Новый тип уведомлений
    /// </summary>
    public NotificationType? Type { get; set; }
    
    /// <summary>
    /// Новая частота отправки
    /// </summary>
    public NotificationFrequency? Frequency { get; set; }
    
    /// <summary>
    /// Новое время для отправки
    /// </summary>
    public DateTime? ScheduledAt { get; set; }
}