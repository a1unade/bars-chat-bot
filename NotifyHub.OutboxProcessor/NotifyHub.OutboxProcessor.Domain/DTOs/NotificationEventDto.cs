using NotifyHub.OutboxProcessor.Domain.Common.Enums;

namespace NotifyHub.OutboxProcessor.Domain.DTOs;

public class NotificationEventDto
{
    /// <summary>
    /// Тип события (создание/удаление/обновление)
    /// </summary>
    public DomainEventType EventType { get; set; }
    
    /// <summary>
    /// Уведомление (создание/обновление)
    /// </summary>
    public NotificationDto? Notification { get; set; }
    
    /// <summary>
    /// Id уведомления (удаление)
    /// </summary>
    public Guid? DeletedId { get; set; }
}