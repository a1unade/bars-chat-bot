using NotifyHub.Domain.DTOs;
using NotifyHub.Domain.Common;

namespace NotifyHub.Domain.Events;

public class NotificationUpdatedDomainEvent(NotificationDto notification) : BaseDomainEvent
{
    public NotificationDto Notification { get; } = notification;
}