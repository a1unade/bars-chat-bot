using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NotifyHub.TelegramBot.Application.Common.Exceptions;
using NotifyHub.TelegramBot.Application.Interfaces;

namespace NotifyHub.TelegramBot.Persistence.Repositories;

public class GenericRepository<T>(IDbContext context) : IGenericRepository<T> where T : class
{
    /// <summary>
    /// Контекст базы данных
    /// </summary>
    private readonly IDbContext _context = context;
    
    /// <summary>
    /// Таблица
    /// </summary>
    private readonly DbSet<T> _set = context.Set<T>();

    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken)
    {
        await _set.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return entity;
    }

    public IQueryable<T> GetAll() => _set.AsNoTracking();

    public IQueryable<T> Get(Expression<Func<T, bool>> predicate) => _set.AsNoTracking().Where(predicate);

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken) => 
        await _set.FindAsync([id], cancellationToken);

    public async Task RemoveByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await GetByIdAsync(id, cancellationToken);

        if (entity is not null)
        {
            _set.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
        else
            throw new NotFoundException(id);
    }

    public async Task<T> UpdateAsync(Guid id, T entity, CancellationToken cancellationToken)
    {
        var ent = await GetByIdAsync(id, cancellationToken);
        
        if (ent is null)
            throw new NotFoundException(id);
        
        _context.Entry(ent).CurrentValues.SetValues(entity);
        await _context.SaveChangesAsync(cancellationToken);
            
        return entity;
    }
}