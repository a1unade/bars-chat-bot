using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NotifyHub.TelegramBot.Application.Interfaces;
using NotifyHub.TelegramBot.Domain.Entities;
using NotifyHub.TelegramBot.Domain.Events;

namespace NotifyHub.TelegramBot.Application.Features.Events;

public class UserCreatedDomainEventHandler(
    IGraphQlService graphQlService,
    IGenericRepository<TelegramUser> repository,
    ILogger<UserCreatedDomainEventHandler> logger): INotificationHandler<UserCreatedDomainEvent>
{
    public async Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var user = notification.User;
        
        var existingUser = await repository
            .Get(u => u.TelegramId == user.TelegramUserId)
            .FirstOrDefaultAsync(cancellationToken);

        if (existingUser is null)
        {
            var userId = await graphQlService.CreateUserAsync(
                name: user.Name,
                telegramUserId: user.TelegramUserId,
                cancellationToken: cancellationToken
            );

            var newUser = new TelegramUser
            {
                Id = userId,
                TelegramId = user.TelegramUserId,
                TelegramTag = user.TelegramTag,
                Name = user.Name,
            };
            
            await repository.AddAsync(newUser, cancellationToken);
            
            logger.LogInformation("User created: {UserId} | @{Username} | {FullName}", newUser.Id, newUser.TelegramTag, newUser.Name);
        }
    }
}