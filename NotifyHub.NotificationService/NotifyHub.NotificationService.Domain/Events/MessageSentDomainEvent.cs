using NotifyHub.Abstractions.DTOs;
using NotifyHub.Abstractions.Events;

namespace NotifyHub.NotificationService.Domain.Events;

public class MessageSentDomainEvent(NotificationMessageDto message) : BaseDomainEvent
{
    public NotificationMessageDto Message { get; } = message;
}