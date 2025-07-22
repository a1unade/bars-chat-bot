using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using NotifyHub.NotificationService.Application.Interfaces;
using NotifyHub.NotificationService.Domain.Common.Enums;
using NotifyHub.NotificationService.Domain.Entities;
using NotifyHub.NotificationService.Domain.Events;

namespace NotifyHub.NotificationService.Application.Features.Events;

public class MessageSentDomainEventHandler(
    ILogger<MessageSentDomainEventHandler> logger,
    IMapper mapper,
    INotificationLogRepository repository): INotificationHandler<MessageSentDomainEvent>
{
    public async Task Handle(MessageSentDomainEvent messageEvent, CancellationToken cancellationToken)
    {
        var message = messageEvent.Message;
        logger.LogInformation("MessageSentDomainEvent triggered: Id={ Id }, Title={ Title }", message.Id, message.Title);
        
        var log = mapper.Map<NotificationLog>(message);
        
        log.SentAt = DateTime.UtcNow;
        log.Status = OperationStatus.Completed;

        await repository.AddAsync(log, cancellationToken);
    }
}