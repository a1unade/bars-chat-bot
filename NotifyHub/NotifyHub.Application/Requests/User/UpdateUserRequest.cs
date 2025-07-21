namespace NotifyHub.Application.Requests.User;

/// <summary>
/// Запрос на обновление пользователя по ID
/// </summary>
public class UpdateUserRequest
{
    /// <summary>
    /// ID пользователя
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    /// ID пользователя в Телеграме
    /// </summary>
    public long? TelegramUserId { get; set; }

    /// <summary>
    /// Почта
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Имя
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Фамилия
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Тег в телеграм
    /// </summary>
    public string? TelegramTag { get; set; }
}