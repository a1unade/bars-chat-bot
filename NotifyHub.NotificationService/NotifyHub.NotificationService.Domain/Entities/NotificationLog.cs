using NotifyHub.NotificationService.Domain.Common;
using NotifyHub.NotificationService.Domain.Common.Enums;

namespace NotifyHub.NotificationService.Domain.Entities;

/// <summary>
/// Лог для завершенных операций
/// </summary>
public class NotificationLog : BaseEntity
{
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