using NotifyHub.TelegramBot.Domain.Common.Enums;

namespace NotifyHub.TelegramBot.Domain.DTOs;

public class CreateNotificationDraft
{
    public string? Title { get; set; }
    
    public string? Description { get; set; }
    
    public string? Type { get; set; } 
    
    public DateTime? ScheduledAt { get; set; }
    
    public int? Frequency { get; set; }
    
    public NotificationCreationStep Step { get; set; } = NotificationCreationStep.Title;
}