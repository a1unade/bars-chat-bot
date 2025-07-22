namespace NotifyHub.Abstractions.Events;

public class NotificationDeletedDomainEvent(Guid notificationId) : BaseDomainEvent
{
    public Guid NotificationId { get; } = notificationId;
}