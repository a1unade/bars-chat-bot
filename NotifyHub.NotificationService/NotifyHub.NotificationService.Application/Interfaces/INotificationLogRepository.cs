using NotifyHub.NotificationService.Domain.Entities;
using System.Linq.Expressions;

namespace NotifyHub.NotificationService.Application.Interfaces;

public interface INotificationLogRepository
{
    /// <summary>
    /// Добавить запись
    /// </summary>
    Task<NotificationLog> AddAsync(NotificationLog entity, CancellationToken cancellationToken);

    /// <summary>
    /// Получить все записи
    /// </summary>
    IQueryable<NotificationLog> GetAll();

    /// <summary>
    /// Получить записи по условию
    /// </summary>
    IQueryable<NotificationLog> Get(Expression<Func<NotificationLog, bool>> predicate);

    /// <summary>
    /// Получить запись по Id
    /// </summary>
    Task<NotificationLog?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Удалить запись по Id
    /// </summary>
    Task RemoveByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Обновить запись по Id
    /// </summary>
    Task<NotificationLog> UpdateAsync(Guid id, NotificationLog entity, CancellationToken cancellationToken);
}