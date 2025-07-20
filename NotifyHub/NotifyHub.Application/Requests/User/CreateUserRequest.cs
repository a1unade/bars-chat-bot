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
    /// Тег в телеграм
    /// </summary>
    public required string TelegramTag { get; set; }
}