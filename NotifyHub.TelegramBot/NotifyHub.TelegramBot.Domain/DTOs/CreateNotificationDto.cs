using NotifyHub.Abstractions.Enums;

namespace NotifyHub.TelegramBot.Domain.DTOs;

public class CreateNotificationDto
{
    /// <summary>
    /// ID пользователя
    /// </summary>
    public Guid UserId { get; set; }
    
    public string Title { get; set; } = null!;
    
    public string Description { get; set; } = null!;
    
    public NotificationType Type { get; set; }
    
    public DateTime ScheduledAt { get; set; }
    
    public NotificationFrequency? Frequency { get; set; }
}