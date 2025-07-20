using NotifyHub.Domain.Common;

namespace NotifyHub.Domain.Events;

public class NotificationDeletedDomainEvent(Guid notificationId) : BaseDomainEvent
{
    public Guid NotificationId { get; } = notificationId;
}