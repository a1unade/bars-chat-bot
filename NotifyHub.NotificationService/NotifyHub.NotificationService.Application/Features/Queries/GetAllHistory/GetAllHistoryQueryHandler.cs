using MediatR;
using Microsoft.EntityFrameworkCore;
using NotifyHub.NotificationService.Application.Common.Requests.History;
using NotifyHub.NotificationService.Application.Interfaces;

namespace NotifyHub.NotificationService.Application.Features.Queries.GetAllHistory;

/// <summary>
/// Обработчик для <see cref="GetAllHistoryQuery"/>
/// </summary>
public class GetAllHistoryQueryHandler(
    INotificationLogRepository repository): IRequestHandler<GetAllHistoryQuery, GetHistoryResponse>
{
    public async Task<GetHistoryResponse> Handle(GetAllHistoryQuery request, CancellationToken cancellationToken)
    {
        var logs = repository.GetAll();

        var responseItems =  await logs
            .Select(log => new GetHistoryResponseItem
            {
                Id = log.Id,
                SentAt = log.SentAt,
                Status = log.Status,
                ErrorMessage = log.ErrorMessage
            })
            .ToListAsync(cancellationToken);

        return new GetHistoryResponse { History = responseItems };
    }
}