using MediatR;
using NotifyHub.NotificationService.Application.Common.Requests.History;

namespace NotifyHub.NotificationService.Application.Features.Queries.GetHistoryById;

public class GetHistoryByIdQueryHandler: IRequestHandler<GetHistoryByIdQuery, GetHistoryByIdResponse>
{
    public async Task<GetHistoryByIdResponse> Handle(GetHistoryByIdQuery request, CancellationToken cancellationToken)
    {
        // TODO: реализовать query handler для запроса записи истории по ID через INotificationLogsRepository, добавить логирование
        
        // TODO: добавить валидатор для ID через пакет FluentValidation
        
        await Task.CompletedTask;

        return new GetHistoryByIdResponse();
    }
}