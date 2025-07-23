using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using NotifyHub.TelegramBot.Application.Interfaces;
using NotifyHub.TelegramBot.Application.Responses;
using NotifyHub.TelegramBot.Infrastructure.Options;

namespace NotifyHub.TelegramBot.Infrastructure.Services;

public class GraphQlService: IGraphQlService
{
    private readonly HttpClient _httpClient;
    private readonly EndpointOptions _options;

    public GraphQlService(HttpClient httpClient, IOptions<EndpointOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

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

        var response = await _httpClient.PostAsJsonAsync(_options.Graphql, query, cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"GraphQL error: {response.StatusCode}, Content: {responseContent}");
        }
        
        var content = await response.Content.ReadFromJsonAsync<GraphQlResponse<CreateUserResponse>>(cancellationToken: cancellationToken);
        return content?.Data.CreateUser ?? Guid.Empty;
    }
}