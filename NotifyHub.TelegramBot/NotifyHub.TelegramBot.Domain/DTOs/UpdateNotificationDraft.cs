using NotifyHub.TelegramBot.Domain.Common.Enums;

namespace NotifyHub.TelegramBot.Domain.DTOs;

public class UpdateNotificationDraft
{
    public Guid? Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Type { get; set; }
    public int? Frequency { get; set; }
    public DateTime? ScheduledAt { get; set; }

    public NotificationUpdateStep Step { get; set; } = NotificationUpdateStep.AskTitle;
}