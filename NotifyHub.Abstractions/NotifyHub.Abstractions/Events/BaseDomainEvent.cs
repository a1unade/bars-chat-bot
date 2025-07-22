using MediatR;

namespace NotifyHub.Abstractions.Events;

public abstract class BaseDomainEvent : INotification
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}