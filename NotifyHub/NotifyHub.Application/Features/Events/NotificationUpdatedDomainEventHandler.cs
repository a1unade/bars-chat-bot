using MediatR;
using Microsoft.Extensions.Logging;
using NotifyHub.Domain.Events;

namespace NotifyHub.Application.Features.Events;

public class NotificationUpdatedDomainEventHandler(ILogger<NotificationUpdatedDomainEventHandler> logger)
    : INotificationHandler<NotificationUpdatedDomainEvent>
{
    public Task Handle(NotificationUpdatedDomainEvent notificationEvent, CancellationToken cancellationToken)
    {
        var notification = notificationEvent.Notification;
        logger.LogInformation("NotificationUpdatedDomainEvent triggered: Id={Id}, Title={Title}, ScheduledAt={ScheduledAt}",
            notification.Id, notification.Title, notification.ScheduledAt);

        // TODO: отправка в Kafka сообщения о создании сущности (NotificationEventDto) с Type = Updated

        return Task.CompletedTask;
    }
}