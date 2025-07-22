using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NotifyHub.Abstractions.Events;
using NotifyHub.OutboxProcessor.Application.Interfaces;
using NotifyHub.OutboxProcessor.Domain.Common.Enums;
using NotifyHub.OutboxProcessor.Domain.Entities;

namespace NotifyHub.OutboxProcessor.Application.Features.Events;

public class NotificationUpdatedDomainEventHandler(
    ILogger<NotificationUpdatedDomainEventHandler> logger,
    IOutboxMessageRepository repository,
    IMapper mapper)
    : INotificationHandler<NotificationUpdatedDomainEvent>
{
    public async Task Handle(NotificationUpdatedDomainEvent notificationEvent, CancellationToken cancellationToken)
    {
        var notification = notificationEvent.Notification;
        logger.LogInformation("NotificationUpdatedDomainEvent triggered: Id={Id}, Title={Title}, ScheduledAt={ScheduledAt}",
            notification.Id, notification.Title, notification.ScheduledAt);

        var outboxMessage = mapper.Map<OutboxMessage>(notification);
                            
        var existingOutboxMessage = await repository.Get(x =>
            x.NotificationId == notification.Id && 
            x.Status != OperationStatus.Sent).FirstOrDefaultAsync(cancellationToken);
                            
        if (existingOutboxMessage is not null)
            await repository.UpdateAsync(existingOutboxMessage.Id, outboxMessage, cancellationToken);
    }
}