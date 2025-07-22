using MediatR;
using NotifyHub.NotificationService.Application.Common.Exceptions;
using NotifyHub.NotificationService.Application.Common.Requests.History;
using NotifyHub.NotificationService.Application.Interfaces;
using NotifyHub.NotificationService.Domain.Entities;

namespace NotifyHub.NotificationService.Application.Features.Queries.GetHistoryById;

public class GetHistoryByIdQueryHandler(INotificationLogRepository repository): 
    IRequestHandler<GetHistoryByIdQuery, GetHistoryByIdResponse>
{
    public async Task<GetHistoryByIdResponse> Handle(GetHistoryByIdQuery request, CancellationToken cancellationToken)
    {
        var logEntry = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (logEntry is null)
            throw new NotFoundException(typeof(NotificationLog));

        return new GetHistoryByIdResponse
        {
            Id = logEntry.Id,
            SentAt = logEntry.SentAt,
            Status = logEntry.Status,
            ErrorMessage = logEntry.ErrorMessage
        };
    }
}