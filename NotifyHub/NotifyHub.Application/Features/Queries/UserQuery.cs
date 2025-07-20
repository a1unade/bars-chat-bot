using NotifyHub.Domain.Entities;
using NotifyHub.Application.Interfaces.Repositories;

namespace NotifyHub.Application.Features.Queries;

[ExtendObjectType("Query")]
public class UserQuery: BaseQuery
{
    [UseSorting]
    [UseFiltering]
    [GraphQLDescription("Получение пользователей")]
    public IQueryable<User> GetUsers([Service] IGenericRepository<User> repository) =>
        GetAll(repository);
}