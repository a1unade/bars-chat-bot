using NotifyHub.TelegramBot.Application.Extensions;
using NotifyHub.TelegramBot.Configurations;
using NotifyHub.TelegramBot.Infrastructure.Extensions;
using NotifyHub.TelegramBot.Persistence.Extensions;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // Mediator
        services.AddApplicationLayer();
        // Сервисы и хэндлеры
        services.AddInfrastructureLayer(context.Configuration);
        // База данных и репозитории
        services.AddPersistenceLayer(context.Configuration);
        
    })
    .Build();

// Применение миграций
await host.UseMigrationsAsync();

await host.RunAsync();