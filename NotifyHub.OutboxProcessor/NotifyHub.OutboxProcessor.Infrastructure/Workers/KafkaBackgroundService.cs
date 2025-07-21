using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NotifyHub.Abstractions.DTOs;
using NotifyHub.Abstractions.Enums;
using NotifyHub.Kafka.Exceptions;
using NotifyHub.Kafka.Interfaces;
using NotifyHub.OutboxProcessor.Application.Interfaces;
using NotifyHub.OutboxProcessor.Domain.Common.Enums;
using NotifyHub.OutboxProcessor.Domain.Entities;

namespace NotifyHub.OutboxProcessor.Infrastructure.Workers;

public class KafkaBackgroundService(
    ILogger<KafkaBackgroundService> logger,
    IKafkaConsumer<NotificationEventDto> consumer,
    IServiceScopeFactory factory,
    IMapper mapper)
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
                    
                    using var scope = factory.CreateScope();
                    var repository = scope.ServiceProvider.GetRequiredService<IOutboxMessageRepository>();
                    
                    switch (message.EventType)
                    {
                        case DomainEventType.Created:
                        {
                            var outboxMessage = mapper.Map<OutboxMessage>(message);
                            await repository.AddAsync(outboxMessage, cancellationToken);
                            
                            break;
                        }
                        case DomainEventType.Updated:
                        {
                            var outboxMessage = mapper.Map<OutboxMessage>(message);
                            
                            var existingOutboxMessage = await repository.Get(x =>
                                x.NotificationId == message.Notification!.Id && 
                                x.Status != OperationStatus.Sent).FirstOrDefaultAsync(cancellationToken);
                            
                            if (existingOutboxMessage is not null)
                                await repository.UpdateAsync(existingOutboxMessage.Id, outboxMessage, cancellationToken);
                            
                            break;
                        }
                        case DomainEventType.Deleted:
                        {
                            var existingOutboxMessage = await repository.Get(x =>
                                x.NotificationId == message.Notification!.Id && 
                                x.Status != OperationStatus.Sent).FirstOrDefaultAsync(cancellationToken);
                            
                            if (existingOutboxMessage is not null)
                                await repository.RemoveByIdAsync(existingOutboxMessage.Id, cancellationToken);
                            
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