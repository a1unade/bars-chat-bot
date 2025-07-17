using NotifyHub.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using NotifyHub.Domain.Common.Enums;
using NotifyHub.Application.Interfaces.Repositories;

namespace NotifyHub.Infrastructure.Processors;

public class OutboxProcessor(IGenericRepository<OutboxMessage> repository, ILogger<OutboxProcessor> logger)
{
    private readonly IGenericRepository<OutboxMessage> _repository = repository;
    private readonly ILogger<OutboxProcessor> _logger = logger;
    
    public async Task ProcessAsync(CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var messages = await _repository
            .Get(x => x.ScheduledAt <= now && x.Status == OperationStatus.Created)
            .Include(x => x.Notification)
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
                
                if (message.Notification.Type == NotificationType.OneTime)
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
        message.Notification.Frequency switch
        {
            NotificationFrequency.Hourly => DateTime.UtcNow.AddHours(1),
            NotificationFrequency.Daily => DateTime.UtcNow.AddDays(1),
            NotificationFrequency.Weekly => DateTime.UtcNow.AddDays(7),
            NotificationFrequency.Monthly => DateTime.UtcNow.AddMonths(1),
            NotificationFrequency.Yearly => DateTime.UtcNow.AddYears(1),
            _ => DateTime.UtcNow
        };
}