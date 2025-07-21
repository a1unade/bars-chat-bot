using MediatR;

namespace NotifyHub.OutboxProcessor.Domain.Common;

public abstract class BaseDomainEvent : INotification
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}