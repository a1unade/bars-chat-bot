using NotifyHub.Application.Interfaces;
using NotifyHub.Domain.Entities;

namespace NotifyHub.Application.Features.Queries;

[ExtendObjectType("Query")]
public class OutboxMessageQuery: BaseQuery
{
    [GraphQLDescription("Получение истории операций по отправке")]
    public IQueryable<OutboxMessage> GetOutboxMessages([Service] IDbContext db) =>
        GetAll<OutboxMessage>(db);
    
    // TODO: заменить контекст на репозиторий
}