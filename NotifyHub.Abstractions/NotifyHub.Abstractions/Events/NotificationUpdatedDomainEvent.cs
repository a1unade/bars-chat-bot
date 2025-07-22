using NotifyHub.Abstractions.DTOs;

namespace NotifyHub.Abstractions.Events;

public class NotificationUpdatedDomainEvent(NotificationDto notification) : BaseDomainEvent
{
    public NotificationDto Notification { get; } = notification;
}