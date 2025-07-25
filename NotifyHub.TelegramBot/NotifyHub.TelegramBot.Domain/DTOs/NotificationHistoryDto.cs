using System.Buffers;

namespace NotifyHub.TelegramBot.Domain.DTOs;

public class NotificationHistoryDto
{
    /// <summary>
    /// Id записи
    /// </summary>
    public Guid Id { get; set; }

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
    
    /// <summary>
    /// Заголовок уведомления
    /// </summary>
    public string? Title { get; set; }
    
    /// <summary>
    /// Описание уведомления
    /// </summary>
    public string? Description { get; set; }
}