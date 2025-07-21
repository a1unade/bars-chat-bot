using Microsoft.EntityFrameworkCore;
using NotifyHub.NotificationService.Application.Common.Exceptions;
using NotifyHub.NotificationService.Application.Interfaces;
using NotifyHub.NotificationService.Domain.Entities;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NotifyHub.NotificationService.Persistence.Repositories;

public class NotificationLogRepository(IDbContext context): INotificationLogRepository
{
    private readonly IDbContext _context = context;

    public async Task<NotificationLog> AddAsync(NotificationLog entity, CancellationToken cancellationToken)
    {
        await _context.NotificationLogs.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public IQueryable<NotificationLog> Get(Expression<Func<NotificationLog, bool>> predicate) => _context.NotificationLogs.AsNoTracking().Where(predicate);

    public IQueryable<NotificationLog> GetAll() => _context.NotificationLogs.AsNoTracking();

    public async Task<NotificationLog?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        await _context.NotificationLogs.FindAsync([id], cancellationToken);

    public async Task RemoveByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await GetByIdAsync(id, cancellationToken);

        if (entity is not null)
        {
            _context.NotificationLogs.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
        else
            throw new NotFoundException(id);
    }

    public async Task<NotificationLog> UpdateAsync(Guid id, NotificationLog entity, CancellationToken cancellationToken)
    {
        var ent = await GetByIdAsync(id, cancellationToken);

        if (ent is null)
            throw new NotFoundException(id);


        _context.NotificationLogs.Update(ent).CurrentValues.SetValues(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

}