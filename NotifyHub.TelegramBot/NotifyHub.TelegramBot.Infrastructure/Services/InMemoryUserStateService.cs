using System.Collections.Concurrent;
using NotifyHub.TelegramBot.Application.Interfaces;
using NotifyHub.TelegramBot.Domain.Common.Enums;

namespace NotifyHub.TelegramBot.Infrastructure.Services;

public class InMemoryUserStateService : IUserStateService
{
    private readonly ConcurrentDictionary<long, UserState> _states = new();

    public UserState GetState(long userId)
        => _states.GetValueOrDefault(userId, UserState.Default);

    public void SetState(long userId, UserState state)
        => _states[userId] = state;

    public void ClearState(long userId)
        => _states.TryRemove(userId, out _);
}