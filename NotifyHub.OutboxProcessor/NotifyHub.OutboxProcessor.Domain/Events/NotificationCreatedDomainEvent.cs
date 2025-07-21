using NotifyHub.Abstractions.DTOs;
using NotifyHub.OutboxProcessor.Domain.Common;

namespace NotifyHub.OutboxProcessor.Domain.Events;

public class NotificationCreatedDomainEvent(NotificationDto notification) : BaseDomainEvent
{
    public NotificationDto Notification { get; } = notification;
}