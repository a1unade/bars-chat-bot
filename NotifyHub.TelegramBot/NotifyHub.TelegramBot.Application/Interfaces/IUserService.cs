using NotifyHub.TelegramBot.Domain.DTOs;

namespace NotifyHub.TelegramBot.Application.Interfaces;

public interface IUserService
{
    /// <summary>
    /// Регистрация пользователя
    /// </summary>
    /// <param name="user">Информация о пользователе</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task RegisterUserAsync(TelegramUserDto user, CancellationToken cancellationToken);
}