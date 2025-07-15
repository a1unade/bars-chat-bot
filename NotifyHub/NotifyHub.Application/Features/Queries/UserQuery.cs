using NotifyHub.Application.Interfaces;
using NotifyHub.Domain.Entities;

namespace NotifyHub.Application.Features.Queries;

[ExtendObjectType("Query")]
public class UserQuery: BaseQuery
{
    [GraphQLDescription("Получение пользователей")]
    public IQueryable<User> GetUsers([Service] IDbContext db) =>
        GetAll<User>(db);
    
    // TODO: заменить контекст на репозиторий
}