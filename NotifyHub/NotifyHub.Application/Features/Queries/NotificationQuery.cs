using NotifyHub.Domain.Entities;
using NotifyHub.Application.Interfaces;

namespace NotifyHub.Application.Features.Queries;

[ExtendObjectType("Query")]
public class NotificationQuery : BaseQuery
{
    [GraphQLDescription("Получение уведомлений")]
    public IQueryable<Notification> GetNotifications([Service] IDbContext db) =>
        GetAll<Notification>(db);
    
    // TODO: заменить контекст на репозиторий
}