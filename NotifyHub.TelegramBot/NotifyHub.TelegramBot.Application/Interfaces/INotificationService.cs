using NotifyHub.TelegramBot.Domain.DTOs;

namespace NotifyHub.TelegramBot.Application.Interfaces;

public interface INotificationService
{
    /// <summary>
    /// Получение списка созданных уведомлений пользователя
    /// </summary>
    /// <param name="telegramUserId">ID пользователя в Telegram</param>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Список созданных уведомлений</returns>
    Task<List<UserNotificationDto>> GetUserNotificationsAsync(long telegramUserId, CancellationToken ct);
    
    /// <summary>
    /// Удаление уведомления по ID
    /// </summary>
    /// <param name="notificationId">ID уведомления, которое необходимо удалить</param>
    /// <param name="ct">Токен отмены</param>
    Task DeleteNotificationAsync(Guid notificationId, CancellationToken ct);
    
    /// <summary>
    /// Создание нового уведомления
    /// </summary>
    /// <param name="telegramUserId">ID пользователя в Telegram</param>
    /// <param name="dto">Данные, необходимые для создания уведомления</param>
    /// <param name="ct">Токен отмены</param>
    /// <returns>ID созданного уведомления</returns>
    Task<Guid> CreateNotificationAsync(long telegramUserId, CreateNotificationDto dto, CancellationToken ct);
    
    /// <summary>
    /// Обновление уведомления пользователя
    /// </summary>
    /// <param name="dto">Данные, необходимые для обновления уведомления</param>
    /// <param name="ct">Токен отмены</param>
    /// <returns>ID обновленного уведомления</returns>
    Task<Guid> UpdateNotificationAsync(UpdateNotificationDto dto, CancellationToken ct);
}