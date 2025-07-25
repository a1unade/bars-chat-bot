using NotifyHub.Abstractions.Enums;
using NotifyHub.TelegramBot.Domain.Common.Enums;

namespace NotifyHub.TelegramBot.Domain.DTOs;

public class UpdateNotificationDraft
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
    public string? Type { get; set; }
    
    /// <summary>
    /// Новая частота отправки
    /// </summary>
    public int? Frequency { get; set; }
    
    /// <summary>
    /// Новое время для отправки
    /// </summary>
    public DateTime? ScheduledAt { get; set; }

    public NotificationUpdateStep Step { get; set; } = NotificationUpdateStep.AskTitle;
}