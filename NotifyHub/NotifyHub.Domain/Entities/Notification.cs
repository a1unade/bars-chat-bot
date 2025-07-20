using NotifyHub.Domain.Common;
using NotifyHub.Domain.Common.Enums;

namespace NotifyHub.Domain.Entities;

/// <summary>
/// Уведомление
/// </summary>
public class Notification: BaseEntity
{
    /// <summary>
    /// Тема
    /// </summary>
    public required string Title { get; set; }
    
    /// <summary>
    /// Описание
    /// </summary>
    public required string Description { get; set; }
    
    /// <summary>
    /// Тип уведомления (одноразовое/периодичное)
    /// </summary>
    public NotificationType Type { get; set; }
    
    /// <summary>
    /// Периодичность отправки уведомления
    /// </summary>
    public NotificationFrequency? Frequency { get; set; }

    /// <summary>
    /// Когда запланирована отправка уведомления
    /// </summary>
    public DateTime ScheduledAt { get; set; }

    /// <summary>
    /// ID пользователя (автор уведомления)
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Навигационное свойство для связи
    /// </summary>
    public User User { get; set; } = default!;
}