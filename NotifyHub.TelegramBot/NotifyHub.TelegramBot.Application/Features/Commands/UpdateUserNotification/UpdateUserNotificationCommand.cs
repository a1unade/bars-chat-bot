using MediatR;
using NotifyHub.TelegramBot.Domain.DTOs;

namespace NotifyHub.TelegramBot.Application.Features.Commands.UpdateUserNotification;

public class UpdateUserNotificationCommand: IRequest<Guid>
{
    public UpdateNotificationDto Dto { get; set; } = null!;
}