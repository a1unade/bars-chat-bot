using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NotifyHub.OutboxProcessor.Application.Interfaces;
using NotifyHub.OutboxProcessor.Domain.Common.Enums;
using NotifyHub.OutboxProcessor.Domain.Entities;

namespace NotifyHub.OutboxProcessor.Infrastructure.Processors;

public class OutboxProcessor(IOutboxMessageRepository repository, ILogger<OutboxProcessor> logger)
{
    private readonly IOutboxMessageRepository _repository = repository;
    private readonly ILogger<OutboxProcessor> _logger = logger;
    
    public async Task ProcessAsync(CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var messages = await _repository
            .Get(x => x.ScheduledAt <= now && x.Status == OperationStatus.Created)
            .ToListAsync(cancellationToken);

        foreach (var message in messages)
        {
            try
            {
                if (message.Status == OperationStatus.Failed)
                    message.Status = OperationStatus.Created;

                message.Status = OperationStatus.InProgress;
                
                _logger.LogInformation("Message sent: {0}, ScheduledAt: {1}", message.PayloadJson, message.ScheduledAt);

                // TODO: прикрутить отправку сообщений в Kafka через Kafka producer

                message.Status = OperationStatus.Sent;
                message.SentAt = DateTime.UtcNow;
                message.ScheduledAt = GetNewScheduledAt(message);
                
                if (message.Type == OperationType.OneTime)
                    await _repository.RemoveByIdAsync(message.Id, cancellationToken);
                else
                    await _repository.UpdateAsync(message.Id, message, cancellationToken);
            }
            catch (TimeoutException timeoutEx)
            {
                _logger.LogError(timeoutEx, "Timeout during sending message ID={Id}", message.Id);
                
                message.Status = OperationStatus.Failed;
                message.Error = timeoutEx.Message;
                // Назначаем повторную отправку сообщения
                message.ScheduledAt = DateTime.UtcNow.AddMinutes(1);
                
                await _repository.UpdateAsync(message.Id, message, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending message ID={Id}", message.Id);
                
                message.Status = OperationStatus.Failed;
                message.Error = ex.Message;
                // Назначаем повторную отправку сообщения
                message.ScheduledAt = DateTime.UtcNow.AddMinutes(1);
                
                await _repository.UpdateAsync(message.Id, message, cancellationToken);
            }
        }
    }
    
    /// <summary>
    /// Получение нового времени для отправки
    /// </summary>
    /// <param name="message">Запись с запланированной отправкой</param>
    /// <returns>Время отправки (когда необходимо отправить сообщение)</returns>
    private DateTime GetNewScheduledAt(OutboxMessage message) =>
        message.Frequency switch
        {
            OperationFrequency.Hourly => DateTime.UtcNow.AddHours(1),
            OperationFrequency.Daily => DateTime.UtcNow.AddDays(1),
            OperationFrequency.Weekly => DateTime.UtcNow.AddDays(7),
            OperationFrequency.Monthly => DateTime.UtcNow.AddMonths(1),
            OperationFrequency.Yearly => DateTime.UtcNow.AddYears(1),
            _ => DateTime.UtcNow
        };
}