using NotifyHub.Domain.Common;
using NotifyHub.Domain.Common.Enums;

namespace NotifyHub.Domain.Entities;

/// <summary>
/// Лог для завершенных операций
/// </summary>
public class NotificationLog : BaseEntity
{
    /// <summary>
    /// ID операции с отправкой уведомления
    /// </summary>
    public Guid OutboxMessageId { get; set; }
    
    /// <summary>
    /// Навигационное свойство для связи
    /// </summary>
    public required OutboxMessage OutboxMessage { get; set; }

    /// <summary>
    /// Во сколько было отправлено
    /// </summary>
    public DateTime SentAt { get; set; }
    
    /// <summary>
    /// Статус операции
    /// </summary>
    public OperationStatus Status { get; set; }
    
    /// <summary>
    /// Сообщение об ошибке (при наличии)
    /// </summary>
    public string? ErrorMessage { get; set; }
}