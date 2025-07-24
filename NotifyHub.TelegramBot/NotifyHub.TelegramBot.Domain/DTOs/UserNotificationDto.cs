namespace NotifyHub.TelegramBot.Domain.DTOs;

public class UserNotificationDto
{
    /// <summary>
    /// ID уведомления
    /// </summary>
    public required Guid Id { get; set; }
    
    /// <summary>
    /// Тема
    /// </summary>
    public required string Title { get; set; }
    
    /// <summary>
    /// Описание
    /// </summary>
    public required string Description { get; set; }
    
    /// <summary>
    /// Тип уведомления (одноразовое/периодичное)
    /// </summary>
    public required string Type { get; set; }
    
    /// <summary>
    /// Периодичность отправки уведомления
    /// </summary>
    public string? Frequency { get; set; }

    /// <summary>
    /// Когда запланирована отправка уведомления
    /// </summary>
    public DateTime ScheduledAt { get; set; }

    /// <summary>
    /// ID пользователя (автор уведомления)
    /// </summary>
    public Guid UserId { get; set; }
}