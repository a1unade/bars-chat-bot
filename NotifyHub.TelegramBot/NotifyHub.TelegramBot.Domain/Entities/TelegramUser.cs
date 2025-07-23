using NotifyHub.TelegramBot.Domain.Common;

namespace NotifyHub.TelegramBot.Domain.Entities;

/// <summary>
/// Пользователь в Telegram
/// </summary>
public class TelegramUser: BaseEntity
{
    /// <summary>
    /// Telegram ID
    /// </summary>
    public long TelegramId { get; set; }
    
    /// <summary>
    /// Telegram tag
    /// </summary>
    public required string TelegramTag { get; set; }
    
    /// <summary>
    /// Имя пользователя
    /// </summary>
    public required string Name { get; set; }
}