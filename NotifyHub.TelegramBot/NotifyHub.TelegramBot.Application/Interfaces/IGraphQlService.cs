using NotifyHub.TelegramBot.Domain.DTOs;

namespace NotifyHub.TelegramBot.Application.Interfaces;

public interface IGraphQlService
{
    /// <summary>
    /// Запрос на создание пользователя
    /// </summary>
    /// <param name="name">Имя пользователя</param>
    /// <param name="telegramUserId">ID пользователя в Telegram</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>ID созданного пользователя</returns>
    Task<Guid> CreateUserAsync(string name, long telegramUserId, CancellationToken cancellationToken);

    /// <summary>
    /// Запрос на получение созданных пользователем уведомлений
    /// </summary>
    /// <param name="userId">ID пользователя</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список созданных уведомлений</returns>
    Task<List<UserNotificationDto>> GetNotificationsAsync(Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Удаление уведомления по ID
    /// </summary>
    /// <param name="id">ID уведомления, которое нужно удалить</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Результат выполнения операции</returns>
    Task<bool> DeleteNotificationAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Создание уведомления 
    /// </summary>
    /// <param name="dto">Данные для создания уведомления</param>
    /// <param name="userId">ID пользователя-автора</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>ID созданного уведомления</returns>
    Task<Guid> CreateNotificationAsync(CreateNotificationDto dto, Guid userId, CancellationToken cancellationToken);
    
    /// <summary>
    /// Обновление уведомления
    /// </summary>
    /// <param name="dto">Данные для обновления уведомления</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>ID обновленного уведомления</returns>
    Task<Guid> UpdateNotificationAsync(UpdateNotificationDto dto, CancellationToken cancellationToken);
}