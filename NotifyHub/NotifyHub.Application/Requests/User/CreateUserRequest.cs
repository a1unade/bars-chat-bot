namespace NotifyHub.Application.Requests.User;

/// <summary>
/// Запрос на создание пользователя
/// </summary>
public class CreateUserRequest
{
    /// <summary>
    /// Почта
    /// </summary>
    public string? Email { get; set; }
    
    /// <summary>
    /// Имя
    /// </summary>
    public required string Name { get; set; }
    
    /// <summary>
    /// ID пользователя в телеграм
    /// </summary>
    public required long TelegramUserId { get; set; }
}