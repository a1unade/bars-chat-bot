using NotifyHub.TelegramBot.Domain.DTOs;

namespace NotifyHub.TelegramBot.Application.Interfaces;

public interface INotificationService
{
    Task<List<UserNotificationDto>> GetUserNotificationsAsync(long telegramUserId, CancellationToken ct);
    
    Task DeleteNotificationAsync(Guid notificationId, CancellationToken ct);
    
    // Task<Guid> CreateNotificationAsync(CreateNotificationDto dto, CancellationToken ct);
}