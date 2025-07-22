using MediatR;
using NotifyHub.Kafka.Interfaces;
using NotifyHub.Abstractions.DTOs;
using NotifyHub.Abstractions.Enums;
using Microsoft.Extensions.Logging;
using NotifyHub.Abstractions.Events;

namespace NotifyHub.Application.Features.Events;

public class NotificationCreatedDomainEventHandler(
    ILogger<NotificationCreatedDomainEventHandler> logger,
    IKafkaProducer<NotificationEventDto> producer)
    : INotificationHandler<NotificationCreatedDomainEvent>
{
    public async Task Handle(NotificationCreatedDomainEvent notificationEvent, CancellationToken cancellationToken)
    {
        var notification = notificationEvent.Notification;
        logger.LogInformation("NotificationCreatedDomainEvent triggered: Id={ Id }, Title={ Title }, ScheduledAt={ ScheduledAt }",
            notification.Id, notification.Title, notification.ScheduledAt);

        var message = new NotificationEventDto
        {
            EventType = DomainEventType.Created,
            Notification = notification,
        };
        
        // Отправка в Kafka
        await producer.ProduceAsync("Outbox", "outbox-key", message, cancellationToken);
        
        logger.LogInformation("Kafka: Message sent to Outbox {0}", message);
    }
}