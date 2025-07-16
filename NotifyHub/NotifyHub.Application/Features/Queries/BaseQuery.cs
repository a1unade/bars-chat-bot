using NotifyHub.Application.Interfaces;
using NotifyHub.Application.Interfaces.Repositories;

namespace NotifyHub.Application.Features.Queries;

public abstract class BaseQuery : IQuery
{
    protected BaseQuery() { }

    protected IQueryable<T> GetAll<T>(IGenericRepository<T> repository) where T : class =>
       repository.GetAll();
}