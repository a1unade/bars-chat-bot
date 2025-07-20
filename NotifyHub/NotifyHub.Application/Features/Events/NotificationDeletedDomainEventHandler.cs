using MediatR;
using NotifyHub.Domain.Events;
using Microsoft.Extensions.Logging;

namespace NotifyHub.Application.Features.Events;

public class NotificationDeletedDomainEventHandler(ILogger<NotificationDeletedDomainEventHandler> logger)
    : INotificationHandler<NotificationDeletedDomainEvent>
{
    public Task Handle(NotificationDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("NotificationDeletedDomainEvent triggered: {NotificationId}", notification.NotificationId);
        
        // TODO: отправка в Kafka сообщения о создании сущности (NotificationEventDto) с Type = Deleted
        return Task.CompletedTask;
    }
}