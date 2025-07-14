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
    /// Тег телеграм
    /// </summary>
    public string? TelegramTag { get; set; }

    /// <summary>
    /// Созданные уведомления
    /// </summary>
    public ICollection<Notification>? Notifications { get; set; }
}