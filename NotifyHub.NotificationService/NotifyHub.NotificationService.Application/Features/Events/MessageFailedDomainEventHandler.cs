using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using NotifyHub.NotificationService.Application.Interfaces;
using NotifyHub.NotificationService.Domain.Common.Enums;
using NotifyHub.NotificationService.Domain.Entities;
using NotifyHub.NotificationService.Domain.Events;

namespace NotifyHub.NotificationService.Application.Features.Events;

public class MessageFailedDomainEventHandler(
    ILogger<MessageFailedDomainEvent> logger,
    IMapper mapper,
    INotificationLogRepository repository): INotificationHandler<MessageFailedDomainEvent>
{
    public async Task Handle(MessageFailedDomainEvent messageEvent, CancellationToken cancellationToken)
    {
        var message = messageEvent.Message;
        logger.LogInformation("MessageFailedDomainEvent triggered: Id={ Id }, Title={ Title }", message.Id, message.Title);
        
        var log = mapper.Map<NotificationLog>(message);
        
        log.SentAt = DateTime.UtcNow;
        log.Status = OperationStatus.Cancelled;
        log.ErrorMessage = messageEvent.Error;

        await repository.AddAsync(log, cancellationToken);
        // TODO: по хорошему делать запись в еще одну таблицу, для которой потом будет ретрай операции отправки
    }
}