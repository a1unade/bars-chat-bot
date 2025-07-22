namespace NotifyHub.Domain.DTOs;

public class UserDto
{
    /// <summary>
    /// ID пользователя
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Почта (для отправки уведомлений на почту)
    /// </summary>
    public string? Email { get; set; }
    
    /// <summary>
    /// Имя
    /// </summary>
    public required string Name { get; set; }
    
    /// <summary>
    /// ID пользователя в Telegram
    /// </summary>
    public required long TelegramUserId { get; set; }
}