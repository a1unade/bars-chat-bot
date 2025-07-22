using System.Text.Json;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NotifyHub.Abstractions.Enums;
using NotifyHub.OutboxProcessor.Application.Interfaces;
using NotifyHub.OutboxProcessor.Domain.Common.Enums;
using NotifyHub.OutboxProcessor.Domain.Entities;
using StackExchange.Redis;

namespace NotifyHub.OutboxProcessor.Infrastructure.Processors;

public class OutboxProcessor(IServiceScopeFactory scopeFactory, ILogger<OutboxProcessor> logger, IConnectionMultiplexer redis)
{
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
    private readonly ILogger<OutboxProcessor> _logger = logger;
    private readonly IConnectionMultiplexer _redis = redis;
    
    [AutomaticRetry(Attempts = 0)]
    public async Task ProcessAsync(CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        
        List<OutboxMessage> messages;
        using (var scope = _scopeFactory.CreateScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<IOutboxMessageRepository>();
            messages = await repository
                .Get(x => x.ScheduledAt <= now && x.Status == OperationStatus.Created)
                .ToListAsync(cancellationToken);
        }

        var semaphore = new SemaphoreSlim(5);

        var tasks = messages.Select(async message =>
        {
            await semaphore.WaitAsync(cancellationToken);
            try
            {
                await ProcessMessageAsync(message, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled error in processing message ID={Id}", message.Id);
            }
            finally
            {
                semaphore.Release();
            }
        });

        await Task.WhenAll(tasks);
    }
    
    private async Task ProcessMessageAsync(OutboxMessage message, CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IOutboxMessageRepository>();
        
        try
        {
            message.Status = OperationStatus.InProgress;
            await repository.UpdateAsync(message.Id, message, cancellationToken);

            _logger.LogInformation("Sending message ID={Id}, ScheduledAt={ScheduledAt}", message.Id, message.ScheduledAt);

            // TODO: Отправка в Kafka через producer

            message.Status = OperationStatus.Sent;
            message.SentAt = DateTime.UtcNow;
            message.ScheduledAt = GetNewScheduledAt(message);

            if (message.Type == NotificationType.OneTime)
                await repository.RemoveByIdAsync(message.Id, cancellationToken);
            else
                await repository.UpdateAsync(message.Id, message, cancellationToken);
        }
        catch (TimeoutException timeoutEx)
        {
            _logger.LogError(timeoutEx, "Timeout during message ID={Id}", message.Id);
            await HandleFailureAsync(message, timeoutEx, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending message ID={Id}", message.Id);
            await HandleFailureAsync(message, ex, cancellationToken);
        }
    }
    
    private async Task HandleFailureAsync(OutboxMessage message, Exception ex, CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IOutboxMessageRepository>();
        
        message.Status = OperationStatus.Failed;
        message.Error = ex.Message;
        message.ScheduledAt = DateTime.UtcNow.AddMinutes(1);
        await repository.UpdateAsync(message.Id, message, cancellationToken);

        // Лог в Redis об ошибке
        await LogFailureToRedisAsync(message, ex);
        
        // Постановка в очередь на ретрай
        BackgroundJob.Schedule<OutboxProcessor>(
            processor => processor.ProcessRetryAsync(message.Id, CancellationToken.None),
            TimeSpan.FromMinutes(1)
        );
    }
    
    /// <summary>
    /// Попытка сделать ретрай для hangfire job
    /// </summary>
    private async Task ProcessRetryAsync(Guid messageId, CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IOutboxMessageRepository>();
        
        var message = await repository.GetByIdAsync(messageId, cancellationToken);
        if (message is null || message.Status != OperationStatus.Failed)
        {
            _logger.LogWarning("Skipping retry: message not found or not failed. ID={Id}", messageId);
            return;
        }

        await ProcessMessageAsync(message, cancellationToken);
    }
    
    /// <summary>
    /// Логирование в Redis
    /// </summary>
    private async Task LogFailureToRedisAsync(OutboxMessage message, Exception ex)
    {
        var db = _redis.GetDatabase();
        var key = $"outbox:failures:{message.Id}";
        var errorLog = JsonSerializer.Serialize(new
        {
            message.Id,
            message.PayloadJson,
            message.ScheduledAt,
            Exception = ex.ToString(),
            Timestamp = DateTime.UtcNow
        });

        await db.StringSetAsync(key, errorLog, TimeSpan.FromDays(7));
    }
    
    /// <summary>
    /// Получение нового времени для отправки
    /// </summary>
    /// <param name="message">Запись с запланированной отправкой</param>
    /// <returns>Время отправки (когда необходимо отправить сообщение)</returns>
    private DateTime GetNewScheduledAt(OutboxMessage message) =>
        message.Frequency switch
        {
            NotificationFrequency.Hourly => DateTime.UtcNow.AddHours(1),
            NotificationFrequency.Daily => DateTime.UtcNow.AddDays(1),
            NotificationFrequency.Weekly => DateTime.UtcNow.AddDays(7),
            NotificationFrequency.Monthly => DateTime.UtcNow.AddMonths(1),
            NotificationFrequency.Yearly => DateTime.UtcNow.AddYears(1),
            _ => DateTime.UtcNow
        };
}