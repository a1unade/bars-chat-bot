namespace NotifyHub.Application.Requests.User;

/// <summary>
/// Запрос на обновление пользователя по ID
/// </summary>
public class UpdateUserRequest
{
    /// <summary>
    /// ID пользователя
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Почта
    /// </summary>
    public string? Email { get; set; }
    
    /// <summary>
    /// Имя
    /// </summary>
    public string? Name { get; set; }
}