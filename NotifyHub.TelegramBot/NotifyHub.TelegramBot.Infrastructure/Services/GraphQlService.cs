using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NotifyHub.TelegramBot.Application.Interfaces;
using NotifyHub.TelegramBot.Application.Responses;
using NotifyHub.TelegramBot.Domain.DTOs;
using NotifyHub.TelegramBot.Infrastructure.Helpers;
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
    
    public async Task<Guid> CreateNotificationAsync(CreateNotificationDto dto, Guid userId, CancellationToken cancellationToken)
    {
        var query = new
        {
            query = @"
            mutation($request: CreateNotificationRequestInput!) {
                createNotification(request: $request) {
                    id
                }
            }
        ",
            variables = new
            {
                request = new
                {
                    userId,
                    title = dto.Title,
                    description = dto.Description,
                    type = ToGraphQlEnumString(dto.Type),
                    frequency = dto.Frequency.HasValue ? ToGraphQlEnumString(dto.Frequency.Value) : null,
                    scheduledAt = dto.ScheduledAt.ToUniversalTime().ToString("o")
                }
            }
        };
        
        logger.LogInformation(query.ToString());

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new GraphQlEnumConverter() }
        };

        var json = JsonSerializer.Serialize(query, options);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        logger.LogInformation("Sending createNotification mutation for UserId: {UserId}", userId);

        var response = await httpClient.PostAsync(_options.Graphql, content, cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("GraphQL error: {StatusCode}, Content: {Content}", response.StatusCode, responseContent);
            throw new HttpRequestException($"GraphQL error: {response.StatusCode}, Content: {responseContent}");
        }

        try
        {
            var parsed = JsonSerializer.Deserialize<GraphQlResponse<CreateNotificationResponse>>(responseContent, options);
            return parsed?.Data.CreateNotification.Id ?? Guid.Empty;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to parse createNotification response. Content: {Content}", responseContent);
            throw;
        }
    }
    
    private string ToGraphQlEnumString(Enum enumValue)
    {
        var name = enumValue.ToString(); 
        
        var builder = new StringBuilder();
        for (int i = 0; i < name.Length; i++)
        {
            var c = name[i];
            if (char.IsUpper(c) && i > 0)
            {
                builder.Append('_');
            }
            builder.Append(char.ToUpperInvariant(c));
        }
        return builder.ToString();
    }
}