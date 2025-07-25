using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NotifyHub.TelegramBot.Application.Features.Events;
using NotifyHub.TelegramBot.Application.Interfaces;
using NotifyHub.TelegramBot.Domain.Entities;

namespace NotifyHub.TelegramBot.Application.Features.Commands.CreateUserNotification;

public class CreateUserNotificationCommandHandler(
    IGraphQlService graphQlService, 
    IGenericRepository<TelegramUser> repository,
    ILogger<UserCreatedDomainEventHandler> logger): IRequestHandler<CreateUserNotificationCommand, Guid>
{
    public async Task<Guid> Handle(CreateUserNotificationCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await repository
            .Get(u => u.TelegramId == request.TelegramUserId)
            .FirstOrDefaultAsync(cancellationToken);

        if (existingUser is not null)
        {
            logger.LogInformation("Notification create command sent by user {0}", request.TelegramUserId);
            
            return await graphQlService.CreateNotificationAsync(
                dto: request.Dto,
                userId: existingUser.Id,
                cancellationToken: cancellationToken
            );
        }
        
        logger.LogInformation("Notification create command reverted {0}", request.TelegramUserId);
        return Guid.Empty;
    }
}