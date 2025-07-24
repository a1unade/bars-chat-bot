using MediatR;

namespace NotifyHub.TelegramBot.Application.Features.Commands.DeleteUserNotification;

public record DeleteUserNotificationCommand: IRequest<bool>
{
    /// <summary>
    /// ID уведомления, которое необходимо удалить
    /// </summary>
    public required Guid NotificationId { get; set; }
}