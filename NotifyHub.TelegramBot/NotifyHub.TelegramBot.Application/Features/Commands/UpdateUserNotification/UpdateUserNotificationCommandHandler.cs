using MediatR;
using Microsoft.Extensions.Logging;
using NotifyHub.TelegramBot.Application.Features.Events;
using NotifyHub.TelegramBot.Application.Interfaces;

namespace NotifyHub.TelegramBot.Application.Features.Commands.UpdateUserNotification;

public class UpdateUserNotificationCommandHandler(
    IGraphQlService graphQlService, 
    ILogger<UserCreatedDomainEventHandler> logger): IRequestHandler<UpdateUserNotificationCommand, Guid>
{
    public async Task<Guid> Handle(UpdateUserNotificationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Notification update command sent for notification {0}", request.Dto.Id);
            return await graphQlService.UpdateNotificationAsync(
                dto: request.Dto,
                cancellationToken: cancellationToken
            );
        }
        catch (Exception ex)
        {
            logger.LogError(
                "Notification update command reverted for notification {0}, Reason: {1}", request.Dto.Id, ex.Message);
            return Guid.Empty;
        }
    }
}