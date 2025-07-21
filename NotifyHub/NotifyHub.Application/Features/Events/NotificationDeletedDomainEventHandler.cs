using MediatR;
using NotifyHub.Domain.Events;
using NotifyHub.Kafka.Interfaces;
using NotifyHub.Abstractions.DTOs;
using NotifyHub.Abstractions.Enums;
using Microsoft.Extensions.Logging;

namespace NotifyHub.Application.Features.Events;

public class NotificationDeletedDomainEventHandler(
    ILogger<NotificationDeletedDomainEventHandler> logger,
    IKafkaProducer<NotificationEventDto> producer)
    : INotificationHandler<NotificationDeletedDomainEvent>
{
    public async Task Handle(NotificationDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("NotificationDeletedDomainEvent triggered: { NotificationId }", notification.NotificationId);

        var message = new NotificationEventDto
        {
            EventType = DomainEventType.Deleted,
            DeletedId = notification.NotificationId,
        };
        
        // Отправка в Kafka
        await producer.ProduceAsync("Outbox", "outbox-key", message, cancellationToken);
        
        logger.LogInformation("Kafka: Message sent to Outbox {0}", message);
    }
}