using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NotifyHub.OutboxProcessor.Application.Common.Exceptions;
using NotifyHub.OutboxProcessor.Application.Interfaces;
using NotifyHub.OutboxProcessor.Domain.Entities;

namespace NotifyHub.OutboxProcessor.Persistence.Repositories;

public class OutboxMessageRepository(IDbContext context): IOutboxMessageRepository
{
    private readonly IDbContext _context = context;
    
    public async Task<OutboxMessage> AddAsync(OutboxMessage entity, CancellationToken cancellationToken)
    {
        await _context.OutboxMessages.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public IQueryable<OutboxMessage> GetAll() => _context.OutboxMessages.AsNoTracking();

    public IQueryable<OutboxMessage> Get(Expression<Func<OutboxMessage, bool>> predicate) =>
        _context.OutboxMessages.AsNoTracking().Where(predicate);

    public async Task<OutboxMessage?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        await _context.OutboxMessages.FindAsync([id], cancellationToken);
    
    public async Task RemoveByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await GetByIdAsync(id, cancellationToken);

        if (entity is not null)
        {
            _context.OutboxMessages.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
        else
            throw new NotFoundException(id);
    }

    public async Task<OutboxMessage> UpdateAsync(Guid id, OutboxMessage entity, CancellationToken cancellationToken)
    {
        var ent = await GetByIdAsync(id, cancellationToken);

        if (ent is null)
            throw new NotFoundException(id);
        
        _context.Entry(ent).CurrentValues.SetValues(entity);
        _context.OutboxMessages.Update(ent); 
        
        await _context.SaveChangesAsync(cancellationToken);

        return ent;
    }
}