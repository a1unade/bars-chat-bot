using System.Net.Http.Json;
using MediatR;
using Microsoft.Extensions.Configuration;
using NotifyHub.TelegramBot.Domain.DTOs;

namespace NotifyHub.TelegramBot.Application.Features.Queries.GetNotificationHistory;

public class GetNotificationHistoryQueryHandler(
    IHttpClientFactory httpClientFactory,
    IConfiguration configuration)
    : IRequestHandler<GetNotificationHistoryQuery, List<NotificationHistoryDto>>
{
    public async Task<List<NotificationHistoryDto>> Handle(GetNotificationHistoryQuery request, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient();

        var baseUrl = configuration["Endpoints:NotificationService"];
        if (string.IsNullOrEmpty(baseUrl))
            throw new InvalidOperationException("NotificationService endpoint is not configured.");

        var url = $"{baseUrl.TrimEnd('/')}/History/{request.TelegramUserId}";

        var response = await client.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();

        var data = await response.Content.ReadFromJsonAsync<List<NotificationHistoryDto>>(cancellationToken: cancellationToken);

        return data ?? [];
    }
}