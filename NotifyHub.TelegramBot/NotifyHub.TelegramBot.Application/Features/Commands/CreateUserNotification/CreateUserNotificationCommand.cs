using MediatR;
using NotifyHub.TelegramBot.Domain.DTOs;

namespace NotifyHub.TelegramBot.Application.Features.Commands.CreateUserNotification;

public class CreateUserNotificationCommand : IRequest<Guid>
{
    public long TelegramUserId { get; set; }
    public CreateNotificationDto Dto { get; set; } = null!;
}