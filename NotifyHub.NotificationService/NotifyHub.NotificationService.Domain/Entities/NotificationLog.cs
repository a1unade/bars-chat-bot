using NotifyHub.NotificationService.Domain.Common;
using NotifyHub.NotificationService.Domain.Common.Enums;

namespace NotifyHub.NotificationService.Domain.Entities;

/// <summary>
/// Лог для отправленных операций
/// </summary>
public class NotificationLog : BaseEntity
{
    /// <summary>
    /// Id уведомления
    /// </summary>
    public required Guid NotificationId { get; set; }
    
    /// <summary>
    /// Заголовок уведомления
    /// </summary>
    public required string Title { get; set; }
    
    /// <summary>
    /// Описание уведомления
    /// </summary>
    public required string Description { get; set; }
    
    /// <summary>
    /// ID пользователя в Telegram
    /// </summary>
    public required long TelegramId { get; set; }
    
    /// <summary>
    /// Почта пользователя
    /// </summary>
    public string? Email { get; set; }
    
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