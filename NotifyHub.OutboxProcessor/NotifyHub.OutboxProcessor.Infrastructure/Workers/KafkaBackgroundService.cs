using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NotifyHub.Kafka.Interfaces;
using NotifyHub.OutboxProcessor.Domain.DTOs;

namespace NotifyHub.OutboxProcessor.Infrastructure.Workers;

public class KafkaBackgroundService(
    ILogger<KafkaBackgroundService> logger,
    IKafkaConsumer<NotificationEventDto> consumer)
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
                    logger.LogInformation("Kafka: message received from Outbox: {Message}", message);
                    // TODO: обработка сообщения
                }
            }
            catch (OperationCanceledException)
            {
                logger.LogInformation("Kafka consumer cancellation requested.");
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