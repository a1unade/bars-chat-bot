using NotifyHub.Abstractions.DTOs;
using NotifyHub.OutboxProcessor.Domain.Common;

namespace NotifyHub.OutboxProcessor.Domain.Events;

public class NotificationUpdatedDomainEvent(NotificationDto notification) : BaseDomainEvent
{
    public NotificationDto Notification { get; } = notification;
}