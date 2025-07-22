using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NotifyHub.Abstractions.DTOs;
using NotifyHub.Kafka.Exceptions;
using NotifyHub.Kafka.Interfaces;

namespace NotifyHub.NotificationService.Infrastructure.Workers;

public class KafkaBackgroundService(
    ILogger<KafkaBackgroundService> logger,
    IKafkaConsumer<NotificationMessageDto> consumer)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Kafka background service started.");

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var message = await consumer.ConsumeAsync("Notification", cancellationToken);

                if (message is not null)
                {
                    logger.LogInformation("Kafka: message received from Notification: {Id}, {Message}", message.Id, message.Title);
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