using MediatR;
using NotifyHub.TelegramBot.Application.Features.Commands.CreateUserNotification;
using NotifyHub.TelegramBot.Application.Features.Commands.DeleteUserNotification;
using NotifyHub.TelegramBot.Application.Features.Queries.GetUserNotifications;
using NotifyHub.TelegramBot.Application.Interfaces;
using NotifyHub.TelegramBot.Domain.DTOs;

namespace NotifyHub.TelegramBot.Infrastructure.Services;

public class NotificationService(IMediator mediator) : INotificationService
{
    public async Task<List<UserNotificationDto>> GetUserNotificationsAsync(long telegramUserId, CancellationToken ct)
    {
        return await mediator.Send(new GetUserNotificationsQuery
        {
            TelegramUserId = telegramUserId
        }, ct);
    }

    public async Task DeleteNotificationAsync(Guid notificationId, CancellationToken ct)
    {
        await mediator.Send(new DeleteUserNotificationCommand
        {
            NotificationId = notificationId
        }, ct);
    }

    public async Task<Guid> CreateNotificationAsync(long telegramUserId, CreateNotificationDto dto, CancellationToken ct)
    {
        return await mediator.Send(new CreateUserNotificationCommand
        {
            TelegramUserId = telegramUserId,
            Dto = dto
        }, ct);
    }
}