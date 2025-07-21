using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using NotifyHub.OutboxProcessor.Application.Interfaces;
using NotifyHub.OutboxProcessor.Domain.Entities;
using NotifyHub.OutboxProcessor.Domain.Events;

namespace NotifyHub.OutboxProcessor.Application.Features.Events;

public class NotificationCreatedDomainEventHandler(
    ILogger<NotificationCreatedDomainEventHandler> logger,
    IOutboxMessageRepository repository,
    IMapper mapper)
    : INotificationHandler<NotificationCreatedDomainEvent>
{
    public async Task Handle(NotificationCreatedDomainEvent notificationEvent, CancellationToken cancellationToken)
    {
        var notification = notificationEvent.Notification;
        logger.LogInformation("NotificationCreatedDomainEvent triggered: Id={ Id }, Title={ Title }, ScheduledAt={ ScheduledAt }",
            notification.Id, notification.Title, notification.ScheduledAt);
        
        var outboxMessage = mapper.Map<OutboxMessage>(notification);
        await repository.AddAsync(outboxMessage, cancellationToken);
    }
}