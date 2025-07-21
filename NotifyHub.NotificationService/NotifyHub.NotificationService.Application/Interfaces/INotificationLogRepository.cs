using NotifyHub.NotificationService.Domain.Entities;
using System.Linq.Expressions;

namespace NotifyHub.NotificationService.Application.Interfaces;

public interface INotificationLogRepository
{
    /// <summary>
    /// Добавить запись
    /// </summary>
    /// <param name="entity">Сущность</param>
    /// <param name="cancellationToken">CancellationToken</param>
    Task<NotificationLog> AddAsync(NotificationLog entity, CancellationToken cancellationToken);

    /// <summary>
    /// Получить все записи о сущности
    /// </summary>
    IQueryable<NotificationLog> GetAll();

    /// <summary>
    /// Получить сущность
    /// </summary>
    /// <param name="predicate">Условие получения</param>
    IQueryable<NotificationLog> Get(Expression<Func<NotificationLog, bool>> predicate);

    /// <summary>
    /// Получить сущность по Id
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="cancellationToken">CancellationToken</param>
    Task<NotificationLog?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Удалить сущность по Id
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="cancellationToken">CancellationToken</param>
    Task RemoveByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Обновить сущность по ID
    /// </summary>
    /// <param name="id">ID сущности</param>
    /// <param name="entity">Сущность</param>
    /// <param name="cancellationToken">CancellationToken</param>
    Task<NotificationLog> UpdateAsync(Guid id, NotificationLog entity, CancellationToken cancellationToken);
}