using System.Linq.Expressions;

namespace NotifyHub.Application.Interfaces.Repositories;

/// <summary>
/// Репозиторий для работы с сущностями
/// </summary>
/// <typeparam name="T">Сущность</typeparam>
public interface IGenericRepository<T> where T : class
{
    /// <summary>
    /// Добавить запись
    /// </summary>
    /// <param name="entity">Сущность</param>
    /// <param name="cancellationToken">CancellationToken</param>
    Task<T> Add(T entity, CancellationToken cancellationToken);

    /// <summary>
    /// Получить все записи о сущности
    /// </summary>
    IQueryable<T> GetAll();

    /// <summary>
    /// Получить сущность
    /// </summary>
    /// <param name="predicate">Условие получения</param>
    IQueryable<T> Get(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Получить сущность по Id
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="cancellationToken">CancellationToken</param>
    Task<T?> GetById(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Удалить сущность по Id
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="cancellationToken">CancellationToken</param>
    Task RemoveById(Guid id, CancellationToken cancellationToken);
    
    /// <summary>
    /// Обновить сущность по ID
    /// </summary>
    /// <param name="id">ID сущности</param>
    /// <param name="entity">Сущность</param>
    /// <param name="cancellationToken">CancellationToken</param>
    Task<T> Update(Guid id, T entity, CancellationToken cancellationToken);
}