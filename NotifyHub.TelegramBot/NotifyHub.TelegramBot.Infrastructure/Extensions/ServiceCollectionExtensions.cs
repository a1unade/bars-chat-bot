using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NotifyHub.TelegramBot.Application.Interfaces;
using NotifyHub.TelegramBot.Infrastructure.Handlers;
using NotifyHub.TelegramBot.Infrastructure.Options;
using NotifyHub.TelegramBot.Infrastructure.Services;
using Telegram.Bot;

namespace NotifyHub.TelegramBot.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddServices(configuration);
        services.AddTelegramBot(configuration);
        services.AddHandlers();
    }

    private static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EndpointOptions>(configuration.GetSection("Endpoints"));
        
        // Register Graphql client
        services.AddHttpClient<IGraphQlService, GraphQlService>();
        
        services.AddScoped<ReceiverService>();
        services.AddHostedService<PollingService>();
    }

    private static void AddTelegramBot(this IServiceCollection services, IConfiguration configuration)
    {
        // Register Bot configuration
        services.Configure<BotOptions>(configuration.GetSection("BotConfiguration"));
        
        // Register named HttpClient to benefits from IHttpClientFactory and consume it with ITelegramBotClient typed client.
        // See https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-5.0#typed-clients
        // and https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests
        services.AddHttpClient("telegram_bot_client").RemoveAllLoggers()
            .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
            {
                BotOptions? botOptions = sp.GetService<IOptions<BotOptions>>()?.Value;
                ArgumentNullException.ThrowIfNull(botOptions);
                TelegramBotClientOptions options = new(botOptions.BotToken);
                return new TelegramBotClient(options, httpClient);
            });
    }

    private static void AddHandlers(this IServiceCollection services)
    {
        services.AddScoped<UpdateHandler>();
    }
}