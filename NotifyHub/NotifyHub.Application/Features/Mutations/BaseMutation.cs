using NotifyHub.Application.Interfaces;
using NotifyHub.Application.Interfaces.Repositories;

namespace NotifyHub.Application.Features.Mutations;

public abstract class BaseMutation: IMutation
{
    protected BaseMutation() { }
    
    protected Task<T> Add<T>(IGenericRepository<T> repository, T entity, CancellationToken cancellationToken) where T : class => 
        repository.Add(entity, cancellationToken);
    
    protected Task<T> Update<T>(IGenericRepository<T> repository, Guid id, T entity, CancellationToken cancellationToken) where T : class =>
        repository.Update(id, entity, cancellationToken);
    
    protected Task Remove<T>(IGenericRepository<T> repository, Guid id, CancellationToken cancellationToken) where T : class =>
        repository.RemoveById(id, cancellationToken);
}