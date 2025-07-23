using NotifyHub.Domain.Common;

namespace NotifyHub.Domain.Entities;

/// <summary>
/// Пользователь
/// </summary>
public class User: BaseEntity
{
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

    /// <summary>
    /// Созданные уведомления
    /// </summary>
    public ICollection<Notification>? Notifications { get; set; }
}