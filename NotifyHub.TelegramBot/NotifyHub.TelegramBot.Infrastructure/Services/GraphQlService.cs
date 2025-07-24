using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NotifyHub.TelegramBot.Application.Interfaces;
using NotifyHub.TelegramBot.Application.Responses;
using NotifyHub.TelegramBot.Domain.DTOs;
using NotifyHub.TelegramBot.Infrastructure.Options;

namespace NotifyHub.TelegramBot.Infrastructure.Services;

public class GraphQlService(
    HttpClient httpClient,
    IOptions<EndpointOptions> options,
    ILogger<GraphQlService> logger)
    : IGraphQlService
{
    private readonly EndpointOptions _options = options.Value;

    public async Task<Guid> CreateUserAsync(string name, long telegramUserId, CancellationToken cancellationToken)
    {
        var query = new
        {
            query = @"
                mutation($request: CreateUserRequestInput!) {
                    createUser(request: $request)
                }
            ",
            variables = new
            {
                request = new
                {
                    name,
                    telegramUserId,
                }
            }
        };

        var response = await httpClient.PostAsJsonAsync(_options.Graphql, query, cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError($"GraphQL error: {response.StatusCode}, Content: {responseContent}");
            throw new HttpRequestException($"GraphQL error: {response.StatusCode}, Content: {responseContent}");
        }
        
        var content = await response.Content.ReadFromJsonAsync<GraphQlResponse<CreateUserResponse>>(cancellationToken: cancellationToken);
        return content?.Data.CreateUser ?? Guid.Empty;
    }
    
    public async Task<List<UserNotificationDto>> GetNotificationsAsync(Guid userId, CancellationToken cancellationToken)
    {
        var query = new
        {
            query = @"
            query($userId: UUID!) {
                notifications(where: { userId: { eq: $userId } }) {
                    id
                    title
                    description
                    type
                    frequency
                    scheduledAt
                    userId
                }
            }",
            variables = new
            {
                userId
            }
        };
        
        logger.LogInformation("Fetching notifications from API, UserId: {0}", userId);

        var response = await httpClient.PostAsJsonAsync(_options.Graphql, query, cancellationToken);
        
        logger.LogInformation("Response: {0}", response);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadFromJsonAsync<GraphQlResponse<NotificationListResponse>>(cancellationToken: cancellationToken);
    
        return content?.Data.Notifications ?? new List<UserNotificationDto>();
    }
    
    public async Task<bool> DeleteNotificationAsync(Guid id, CancellationToken cancellationToken)
    {
        var query = new
        {
            query = @"
            mutation($id: UUID!) {
                deleteNotification(id: $id)
            }
        ",
            variables = new { id }
        };

        logger.LogInformation("Sending deleteNotification mutation for NotificationId: {NotificationId}", id);

        var response = await httpClient.PostAsJsonAsync(_options.Graphql, query, cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("GraphQL error: {StatusCode}, Content: {Content}", response.StatusCode, responseContent);
            throw new HttpRequestException($"GraphQL error: {response.StatusCode}, Content: {responseContent}");
        }

        try
        {
            var content = await response.Content.ReadFromJsonAsync<GraphQlResponse<DeleteNotificationResponse>>(cancellationToken: cancellationToken);
            return content?.Data.DeleteNotification ?? false;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to parse deleteNotification response. Content: {Content}", responseContent);
            throw;
        }
    }
}