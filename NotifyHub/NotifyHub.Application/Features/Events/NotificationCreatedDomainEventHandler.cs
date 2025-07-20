using MediatR;
using NotifyHub.Domain.Events;
using Microsoft.Extensions.Logging;

namespace NotifyHub.Application.Features.Events;

public class NotificationCreatedDomainEventHandler(ILogger<NotificationCreatedDomainEventHandler> logger)
    : INotificationHandler<NotificationCreatedDomainEvent>
{
    public Task Handle(NotificationCreatedDomainEvent notificationEvent, CancellationToken cancellationToken)
    {
        var notification = notificationEvent.Notification;
        logger.LogInformation("NotificationCreatedDomainEvent triggered: Id={Id}, Title={Title}, ScheduledAt={ScheduledAt}",
            notification.Id, notification.Title, notification.ScheduledAt);

        // TODO: отправка в Kafka сообщения о создании сущности (NotificationEventDto) с Type = Created

        return Task.CompletedTask;
    }
}