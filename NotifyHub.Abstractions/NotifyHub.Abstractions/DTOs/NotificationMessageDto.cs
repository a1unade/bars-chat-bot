namespace NotifyHub.Abstractions.DTOs;

/// <summary>
/// Сообщение для NotificationService
/// </summary>
public class NotificationMessageDto
{
    /// <summary>
    /// ID уведомления
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Описание
    /// </summary>
    public required string Description { get; set; }
    
    /// <summary>
    /// Название
    /// </summary>
    public required string Title { get; set; }
    
    /// <summary>
    /// ID пользователя в Telegram
    /// </summary>
    public required string TelegramTag { get; set; }
    
    /// <summary>
    /// Почта пользователя, если потребуется отправка по почте
    /// </summary>
    public string? Email { get; init; }
}