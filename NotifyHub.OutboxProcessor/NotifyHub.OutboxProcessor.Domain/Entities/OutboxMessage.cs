using NotifyHub.OutboxProcessor.Domain.Common;
using NotifyHub.OutboxProcessor.Domain.Common.Enums;

namespace NotifyHub.OutboxProcessor.Domain.Entities;

/// <summary>
/// Операция по отправке уведомления
/// </summary>
public class OutboxMessage: BaseEntity
{
    /// <summary>
    /// ID уведомления
    /// </summary>
    public required Guid NotificationId { get; set; }
    
    /// <summary>
    /// Тип отправки (периодическая/одноразовая)
    /// </summary>
    public required OperationType Type { get; set; }
    
    /// <summary>
    /// Частота отправки
    /// </summary>
    public OperationFrequency? Frequency { get; set; }

    /// <summary>
    /// Когда запланирована отправка
    /// </summary>
    public required DateTime ScheduledAt { get; set; }
    
    /// <summary>
    /// Когда было отправлено
    /// </summary>
    public DateTime? SentAt { get; set; }

    /// <summary>
    /// Статус отправки
    /// </summary>
    public required OperationStatus Status { get; set; } = OperationStatus.Created;
    
    /// <summary>
    /// Сообщение об ошибке
    /// </summary>
    public string? Error { get; set; }

    /// <summary>
    /// Сообщение для отправки
    /// </summary>
    public required string PayloadJson { get; set; }
}