using NotifyHub.Abstractions.DTOs;
using NotifyHub.Abstractions.Events;

namespace NotifyHub.NotificationService.Domain.Events;

public class MessageFailedDomainEvent(NotificationMessageDto message, string error) : BaseDomainEvent
{
    public NotificationMessageDto Message { get; } = message;
    
    public string Error { get; } = error;
}