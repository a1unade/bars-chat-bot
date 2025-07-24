using MediatR;
using NotifyHub.TelegramBot.Application.Interfaces;
using NotifyHub.TelegramBot.Domain.DTOs;
using NotifyHub.TelegramBot.Domain.Events;

namespace NotifyHub.TelegramBot.Infrastructure.Services;

public class UserService: IUserService
{
    private readonly IMediator _mediator;

    public UserService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task RegisterUserAsync(TelegramUserDto user, CancellationToken cancellationToken) =>
        await _mediator.Publish(new UserCreatedDomainEvent(user), cancellationToken);
}