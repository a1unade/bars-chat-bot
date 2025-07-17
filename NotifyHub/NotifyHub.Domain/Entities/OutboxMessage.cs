using NotifyHub.Domain.Common;
using NotifyHub.Domain.Common.Enums;

namespace NotifyHub.Domain.Entities;

/// <summary>
/// Операция по отправке уведомления
/// </summary>
public class OutboxMessage: BaseEntity
{
    /// <summary>
    /// ID уведомления
    /// </summary>
    public Guid NotificationId { get; set; }

    /// <summary>
    /// Когда запланирована отправка
    /// </summary>
    public DateTime ScheduledAt { get; set; }
    
    /// <summary>
    /// Когда было отправлено
    /// </summary>
    public DateTime? SentAt { get; set; }

    /// <summary>
    /// Статус отправки
    /// </summary>
    public OperationStatus Status { get; set; } = OperationStatus.Created;
    
    /// <summary>
    /// Сообщение об ошибке
    /// </summary>
    public string? Error { get; set; }

    /// <summary>
    /// Сообщение для отправки
    /// </summary>
    public required string PayloadJson { get; set; }
}