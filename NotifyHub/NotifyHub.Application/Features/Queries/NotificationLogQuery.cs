using NotifyHub.Domain.Entities;
using NotifyHub.Application.Interfaces.Repositories;

namespace NotifyHub.Application.Features.Queries;

[ExtendObjectType("Query")]
public class NotificationLogQuery: BaseQuery
{
    [GraphQLDescription("Получение истории уведомлений")]
    public IQueryable<NotificationLog> GetNotificationLogs([Service] IGenericRepository<NotificationLog> repository) =>
        GetAll(repository);
}