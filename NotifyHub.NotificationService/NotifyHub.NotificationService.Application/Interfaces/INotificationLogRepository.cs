using NotifyHub.NotificationService.Domain.Entities;
using System.Linq.Expressions;

namespace NotifyHub.NotificationService.Application.Interfaces;

public interface INotificationLogRepository
{
    /// <summary>
    /// �������� ������
    /// </summary>
    /// <param name="entity">��������</param>
    /// <param name="cancellationToken">CancellationToken</param>
    Task<NotificationLog> AddAsync(NotificationLog entity, CancellationToken cancellationToken);

    /// <summary>
    /// �������� ��� ������ � ��������
    /// </summary>
    IQueryable<NotificationLog> GetAll();

    /// <summary>
    /// �������� ��������
    /// </summary>
    /// <param name="predicate">������� ���������</param>
    IQueryable<NotificationLog> Get(Expression<Func<NotificationLog, bool>> predicate);

    /// <summary>
    /// �������� �������� �� Id
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="cancellationToken">CancellationToken</param>
    Task<NotificationLog?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// ������� �������� �� Id
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="cancellationToken">CancellationToken</param>
    Task RemoveByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// �������� �������� �� ID
    /// </summary>
    /// <param name="id">ID ��������</param>
    /// <param name="entity">��������</param>
    /// <param name="cancellationToken">CancellationToken</param>
    Task<NotificationLog> UpdateAsync(Guid id, NotificationLog entity, CancellationToken cancellationToken);
}