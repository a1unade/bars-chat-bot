using NotifyHub.Domain.Entities;
using NotifyHub.Application.Interfaces;

namespace NotifyHub.Application.Features.Queries;

[ExtendObjectType("Query")]
public class NotificationQuery : BaseQuery, IQuery
{
    [UseSorting]
    [UseFiltering]
    [GraphQLDescription("Получение уведомлений")]
    public IQueryable<Notification> GetNotifications([Service] IDbContext db) =>
        GetAll<Notification>(db);
}