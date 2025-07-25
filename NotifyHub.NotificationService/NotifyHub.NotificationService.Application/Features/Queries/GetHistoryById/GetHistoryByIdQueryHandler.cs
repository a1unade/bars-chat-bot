using MediatR;
using Microsoft.EntityFrameworkCore;
using NotifyHub.NotificationService.Application.Common.Requests.History;
using NotifyHub.NotificationService.Application.Interfaces;

namespace NotifyHub.NotificationService.Application.Features.Queries.GetHistoryById;

public class GetHistoryByIdQueryHandler(INotificationLogRepository repository)
    : IRequestHandler<GetHistoryByIdQuery, GetHistoryByIdResponse[]>
{
    public async Task<GetHistoryByIdResponse[]> Handle(GetHistoryByIdQuery request, CancellationToken cancellationToken)
    {
        Console.WriteLine($"[Handler] Запрос истории по TelegramId = {request.Id}");

        var logs = await repository
            .Get(x => x.TelegramId == request.Id)
            .AsQueryable()
            .ToArrayAsync(cancellationToken);

        Console.WriteLine($"[Handler] Найдено записей: {logs.Length}");

        if (logs.Length == 0)
            return [];

        return logs.Select(logEntry => new GetHistoryByIdResponse
        {
            Id = logEntry.Id,
            SentAt = logEntry.SentAt,
            Status = logEntry.Status,
            ErrorMessage = logEntry.ErrorMessage,
            Title = logEntry.Title,
            Description = logEntry.Description
        }).ToArray();
    }

}