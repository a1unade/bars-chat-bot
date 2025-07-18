using System.Linq.Expressions;
using NotifyHub.OutboxProcessor.Domain.Entities;

namespace NotifyHub.OutboxProcessor.Application.Interfaces;

public interface IOutboxMessageRepository
{
    /// <summary>
    /// Добавить запись
    /// </summary>
    Task<OutboxMessage> AddAsync(OutboxMessage entity, CancellationToken cancellationToken);

    /// <summary>
    /// Получить все записи
    /// </summary>
    IQueryable<OutboxMessage> GetAll();

    /// <summary>
    /// Получить записи по условию
    /// </summary>
    IQueryable<OutboxMessage> Get(Expression<Func<OutboxMessage, bool>> predicate);

    /// <summary>
    /// Получить запись по Id
    /// </summary>
    Task<OutboxMessage?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Удалить запись по Id
    /// </summary>
    Task RemoveByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Обновить запись по Id
    /// </summary>
    Task<OutboxMessage> UpdateAsync(Guid id, OutboxMessage entity, CancellationToken cancellationToken);
}