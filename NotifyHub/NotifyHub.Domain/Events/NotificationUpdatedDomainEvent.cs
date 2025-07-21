using NotifyHub.Domain.Common;
using NotifyHub.Abstractions.DTOs;

namespace NotifyHub.Domain.Events;

public class NotificationUpdatedDomainEvent(NotificationDto notification) : BaseDomainEvent
{
    public NotificationDto Notification { get; } = notification;
}