using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NotifyHub.Abstractions.DTOs;
using NotifyHub.Kafka.Exceptions;
using NotifyHub.Kafka.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace NotifyHub.TelegramBot.Infrastructure.Workers;

public class KafkaBackgroundService(
    ILogger<KafkaBackgroundService> logger,
    IKafkaConsumer<NotificationMessageDto> consumer,
    ITelegramBotClient botClient)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Kafka background service started.");

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var message = await consumer.ConsumeAsync("Bot", cancellationToken);

                if (message is not null)
                {
                    logger.LogInformation("Kafka: message received from Notification: {Id}, {Message}", message.Id, message.Title);
                    
                    try
                    {
                        await botClient.SendMessage(
                            chatId: message.TelegramUserId,
                            text: $"*{message.Title}*\n{message.Description}",
                            parseMode: ParseMode.Markdown,
                            cancellationToken: cancellationToken);

                        logger.LogInformation("Telegram message sent to user {UserId}", message.TelegramUserId);
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, "Failed to send Telegram message to user {UserId}", message.TelegramUserId);
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