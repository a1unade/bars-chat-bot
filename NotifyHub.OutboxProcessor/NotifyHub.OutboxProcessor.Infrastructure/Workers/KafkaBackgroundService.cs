using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NotifyHub.Abstractions.DTOs;
using NotifyHub.Abstractions.Enums;
using NotifyHub.Kafka.Exceptions;
using NotifyHub.Kafka.Interfaces;
using NotifyHub.OutboxProcessor.Domain.Events;

namespace NotifyHub.OutboxProcessor.Infrastructure.Workers;

public class KafkaBackgroundService(
    ILogger<KafkaBackgroundService> logger,
    IKafkaConsumer<NotificationEventDto> consumer,
    IServiceScopeFactory scopeFactory)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Kafka background service started.");

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var message = await consumer.ConsumeAsync("Outbox", cancellationToken);

                if (message is not null)
                {
                    logger.LogInformation("Kafka: message received from Outbox: {Type}, {Message}", message.EventType, message.Notification?.Id);
                    
                    using var scope = scopeFactory.CreateScope();

                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    
                    switch (message.EventType)
                    {
                        case DomainEventType.Created:
                        {
                            if (message.Notification is not null)
                                await mediator.Publish(new NotificationCreatedDomainEvent(message.Notification!), cancellationToken);
                            
                            break;
                        }
                        case DomainEventType.Updated:
                        {
                            if (message.Notification is not null)
                                await mediator.Publish(new NotificationUpdatedDomainEvent(message.Notification!), cancellationToken);
                            
                            break;
                        }
                        case DomainEventType.Deleted:
                        {
                            if (message.DeletedId is not null)
                                await mediator.Publish(new NotificationDeletedDomainEvent(message.DeletedId.Value), cancellationToken);
                            
                            break;
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                logger.LogInformation("Kafka consumer cancellation requested.");
                break;
            }
            catch (KafkaConsumeException ex)
            {
                logger.LogError("Failed to consume Kafka message.{0}", ex.Message);
                break;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error while consuming Kafka message.");
            }
        }

        try
        {
            consumer.Close();
            logger.LogInformation("Kafka consumer closed.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error closing Kafka consumer.");
        }
    }
}