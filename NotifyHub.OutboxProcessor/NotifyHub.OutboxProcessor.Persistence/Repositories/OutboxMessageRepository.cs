using System.Linq.Expressions;
using NotifyHub.OutboxProcessor.Application.Interfaces;
using NotifyHub.OutboxProcessor.Domain.Entities;

namespace NotifyHub.OutboxProcessor.Persistence.Repositories;

public class OutboxMessageRepository: IOutboxMessageRepository
{
    public async Task<OutboxMessage> AddAsync(OutboxMessage entity, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }

    public IQueryable<OutboxMessage> GetAll()
    {
        throw new NotImplementedException();
    }

    public IQueryable<OutboxMessage> Get(Expression<Func<OutboxMessage, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public async Task<OutboxMessage?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }

    public async Task RemoveByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }

    public async Task<OutboxMessage> UpdateAsync(Guid id, OutboxMessage entity, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }
}