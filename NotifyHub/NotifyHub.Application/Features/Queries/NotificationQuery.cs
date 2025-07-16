using NotifyHub.Domain.Entities;
using NotifyHub.Application.Interfaces.Repositories;

namespace NotifyHub.Application.Features.Queries;

[ExtendObjectType("Query")]
public class NotificationQuery : BaseQuery
{
    [GraphQLDescription("Получение уведомлений")]
    public IQueryable<Notification> GetNotifications([Service] IGenericRepository<Notification> repository) =>
        GetAll(repository);
}