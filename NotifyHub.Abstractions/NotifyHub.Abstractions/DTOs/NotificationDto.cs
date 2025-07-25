using NotifyHub.Abstractions.Enums;

namespace NotifyHub.Abstractions.DTOs;

/// <summary>
/// DTO для передачи уведомления
/// </summary>
public class NotificationDto
{
    /// <summary>
    /// ID
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Название
    /// </summary>
    public required string Title { get; set; }
    
    /// <summary>
    /// Описание
    /// </summary>
    public required string Description { get; set; }
    
    /// <summary>
    /// Тип (периодичное/одноразовое)
    /// </summary>
    public NotificationType Type { get; set; }
    
    /// <summary>
    /// Частота отправки
    /// </summary>
    public NotificationFrequency? Frequency { get; set; }
    
    /// <summary>
    /// Когда запланирована отправка
    /// </summary>
    public DateTime ScheduledAt { get; set; }
    
    /// <summary>
    /// ID пользователя в Telegram
    /// </summary>
    public required long TelegramUserId { get; set; }
    
    /// <summary>
    /// Почта пользователя, если потребуется отправка по почте
    /// </summary>
    public string? Email { get; init; }
}