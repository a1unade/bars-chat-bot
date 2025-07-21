using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NotifyHub.OutboxProcessor.Application.Interfaces;
using NotifyHub.OutboxProcessor.Domain.Common.Enums;
using NotifyHub.OutboxProcessor.Domain.Events;

namespace NotifyHub.OutboxProcessor.Application.Features.Events;

public class NotificationDeletedDomainEventHandler(
    ILogger<NotificationDeletedDomainEventHandler> logger,
    IOutboxMessageRepository repository)
    : INotificationHandler<NotificationDeletedDomainEvent>
{
    public async Task Handle(NotificationDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("NotificationDeletedDomainEvent triggered: { NotificationId }", notification.NotificationId);
        
        var existingOutboxMessage = await repository.Get(x =>
            x.NotificationId == notification.NotificationId && 
            x.Status != OperationStatus.Sent).FirstOrDefaultAsync(cancellationToken);
        
        if (existingOutboxMessage is not null)
            await repository.RemoveByIdAsync(existingOutboxMessage.Id, cancellationToken);
    }
}