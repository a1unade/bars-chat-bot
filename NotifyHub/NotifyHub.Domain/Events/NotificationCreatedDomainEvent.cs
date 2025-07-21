using NotifyHub.Domain.DTOs;
using NotifyHub.Domain.Common;

namespace NotifyHub.Domain.Events;

public class NotificationCreatedDomainEvent(NotificationDto notification) : BaseDomainEvent
{
    public NotificationDto Notification { get; } = notification;
}
