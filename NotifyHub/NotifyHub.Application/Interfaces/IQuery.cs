namespace NotifyHub.Application.Interfaces;

public interface IQuery
{
    IQueryable<T> GetAll<T>([Service] IDbContext db) where T : class;
}