using NotifyHub.Domain.Common;
using NotifyHub.Abstractions.DTOs;

namespace NotifyHub.Domain.Events;

public class NotificationCreatedDomainEvent(NotificationDto notification) : BaseDomainEvent
{
    public NotificationDto Notification { get; } = notification;
}
