using NotifyHub.Abstractions.Events;
using NotifyHub.TelegramBot.Domain.DTOs;

namespace NotifyHub.TelegramBot.Domain.Events;

/// <summary>
/// Событие регистрации пользователя
/// </summary>
/// <param name="user">Данные о пользователе из Telegram бота</param>
public class UserCreatedDomainEvent(TelegramUserDto user): BaseDomainEvent
{
    public TelegramUserDto User { get; } = user;
}