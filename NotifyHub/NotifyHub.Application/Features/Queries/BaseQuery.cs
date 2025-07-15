using Microsoft.EntityFrameworkCore;
using NotifyHub.Application.Interfaces;

namespace NotifyHub.Application.Features.Queries;

public abstract class BaseQuery : IQuery
{
    protected BaseQuery() { }

    protected IQueryable<T> GetAll<T>(IDbContext db) where T : class =>
        db.Set<T>().AsNoTracking();
    
    // TODO: заменить контекст на репозиторий
}