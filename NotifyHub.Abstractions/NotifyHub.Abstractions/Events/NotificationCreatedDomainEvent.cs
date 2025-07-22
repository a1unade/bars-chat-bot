using NotifyHub.Abstractions.DTOs;

namespace NotifyHub.Abstractions.Events;

public class NotificationCreatedDomainEvent(NotificationDto notification) : BaseDomainEvent
{
    public NotificationDto Notification { get; } = notification;
}