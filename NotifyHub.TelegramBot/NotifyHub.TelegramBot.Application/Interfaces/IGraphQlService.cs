namespace NotifyHub.TelegramBot.Application.Interfaces;

public interface IGraphQlService
{
    /// <summary>
    /// Запрос на создание пользователя
    /// </summary>
    /// <param name="name">Имя пользователя</param>
    /// <param name="telegramUserId">ID пользователя в Telegram</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>ID созданного пользователя</returns>
    Task<Guid> CreateUserAsync(string name, long telegramUserId, CancellationToken cancellationToken);
}