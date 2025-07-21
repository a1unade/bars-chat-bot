using NotifyHub.OutboxProcessor.Domain.Common;

namespace NotifyHub.OutboxProcessor.Domain.Events;

public class NotificationDeletedDomainEvent(Guid notificationId) : BaseDomainEvent
{
    public Guid NotificationId { get; } = notificationId;
}