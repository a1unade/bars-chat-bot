using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NotifyHub.TelegramBot.Application.Interfaces;
using NotifyHub.TelegramBot.Domain.DTOs;
using NotifyHub.TelegramBot.Domain.Entities;

namespace NotifyHub.TelegramBot.Application.Features.Queries.GetUserNotifications;

public class GetUserNotificationsHandler(
    IGraphQlService graphQlService,
    IGenericRepository<TelegramUser> repository,
    ILogger<GetUserNotificationsHandler> logger): IRequestHandler<GetUserNotificationsQuery, List<UserNotificationDto>>
{
    public async Task<List<UserNotificationDto>> Handle(GetUserNotificationsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("GetUserNotificationsHandler");
        logger.LogInformation(request.TelegramUserId.ToString());
        var existingUser = await repository
            .Get(u => u.TelegramId == request.TelegramUserId)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (existingUser is null)
            return new List<UserNotificationDto>();
        try
        {
            logger.LogInformation("Fetching user notifications from API, UserId: {UserId}", existingUser.Id);

            var result = await graphQlService.GetNotificationsAsync(existingUser.Id, cancellationToken);

            logger.LogInformation("Fetched {Count} notifications", result?.Count ?? 0);

            return result ?? [];
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while fetching notifications from GraphQL API");
            return [];
        }
    }
}