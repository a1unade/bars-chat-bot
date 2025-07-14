using Microsoft.EntityFrameworkCore;
using NotifyHub.Application.Interfaces;

namespace NotifyHub.Application.Features.Queries;

[ExtendObjectType("Query")]
public abstract class BaseQuery
{
    [UseSorting]
    [UseFiltering]
    [GraphQLDescription("Получение всех данных указанного типа")]
    public virtual IQueryable<T> GetAll<T>([Service] IDbContext db) where T : class =>
        db.Set<T>().AsNoTracking();
}