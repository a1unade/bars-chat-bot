using MediatR;
using NotifyHub.NotificationService.Application.Common.Requests.History;

namespace NotifyHub.NotificationService.Application.Features.Queries.GetAllHistory;

/// <summary>
/// Обработчик для <see cref="GetAllHistoryQuery"/>
/// </summary>
public class GetAllHistoryQueryHandler: IRequestHandler<GetAllHistoryQuery, GetHistoryResponse>
{
    public async Task<GetHistoryResponse> Handle(GetAllHistoryQuery request, CancellationToken cancellationToken)
    {
        // TODO: реализовать query handler для запроса истории через INotificationLogsRepository, добавить логирование
        await Task.CompletedTask;

        return new GetHistoryResponse { History = [] };
    }
}