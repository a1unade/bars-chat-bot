using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NotifyHub.Application.Interfaces;
using NotifyHub.Application.Common.Exceptions; 
using NotifyHub.Application.Interfaces.Repositories;

namespace NotifyHub.Persistence.Repositories;

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

    public async Task<T> Add(T entity, CancellationToken cancellationToken)
    {
        await _set.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return entity;
    }

    public IQueryable<T> GetAll() => _set.AsNoTracking();

    public IQueryable<T> Get(Expression<Func<T, bool>> predicate) => _set.AsNoTracking().Where(predicate);

    public async Task<T?> GetById(Guid id, CancellationToken cancellationToken) => 
        await _set.FindAsync([id], cancellationToken);

    public async Task RemoveById(Guid id, CancellationToken cancellationToken)
    {
        var entity = await GetById(id, cancellationToken);

        if (entity is not null)
        {
            _set.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
        else
            throw new NotFoundException(id);
    }

    public async Task<T> Update(Guid id, T entity, CancellationToken cancellationToken)
    {
        var ent = await GetById(id, cancellationToken);
        
        if (ent is null)
            throw new NotFoundException(id);
        
        _context.Entry(ent).CurrentValues.SetValues(entity);
        await _context.SaveChangesAsync(cancellationToken);
            
        return entity;
    }
}