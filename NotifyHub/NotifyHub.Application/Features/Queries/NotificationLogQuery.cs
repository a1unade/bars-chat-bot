using NotifyHub.Application.Interfaces;
using NotifyHub.Domain.Entities;

namespace NotifyHub.Application.Features.Queries;

[ExtendObjectType("Query")]
public class NotificationLogQuery: BaseQuery
{
    [GraphQLDescription("Получение истории уведомлений")]
    public IQueryable<NotificationLog> GetNotificationLogs([Service] IDbContext db) =>
        GetAll<NotificationLog>(db);
    
    // TODO: заменить контекст на репозиторий
}