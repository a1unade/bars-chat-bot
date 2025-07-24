using MediatR;
using NotifyHub.TelegramBot.Application.Interfaces;

namespace NotifyHub.TelegramBot.Application.Features.Commands.DeleteUserNotification;

public class DeleteUserNotificationHandler(
    IGraphQlService graphQlService): IRequestHandler<DeleteUserNotificationCommand, bool>
{
    public async Task<bool> Handle(DeleteUserNotificationCommand request, CancellationToken cancellationToken)
    {
        return await graphQlService.DeleteNotificationAsync(request.NotificationId, cancellationToken);
    }
}