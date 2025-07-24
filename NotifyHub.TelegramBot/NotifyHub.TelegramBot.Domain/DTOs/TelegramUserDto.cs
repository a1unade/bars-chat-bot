namespace NotifyHub.TelegramBot.Domain.DTOs;

/// <summary>
/// Данные о пользователе из Telegram бота
/// </summary>
public class TelegramUserDto
{
    /// <summary>
    /// ID пользователя в Telegram
    /// </summary>
    public required long TelegramUserId { get; set; }
    
    /// <summary>
    /// Тег пользователя в Telegram
    /// </summary>
    public required string TelegramTag { get; set; }
    
    /// <summary>
    /// Имя пользователя в Telegram
    /// </summary>
    public required string Name { get; set; }
}