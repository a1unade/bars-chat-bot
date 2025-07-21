using Hangfire;
using Hangfire.MemoryStorage;
using NotifyHub.Kafka.Extensions;
using NotifyHub.Abstractions.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotifyHub.OutboxProcessor.Infrastructure.Jobs;
using NotifyHub.OutboxProcessor.Infrastructure.Workers;

namespace NotifyHub.OutboxProcessor.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfireJobs();
        services.AddKafka(configuration);
        services.AddWorkers();
    }

    private static void AddKafka(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddKafkaConsumer<NotificationEventDto>(configuration);
    }

    private static void AddWorkers(this IServiceCollection services)
    {
        services.AddHostedService<KafkaBackgroundService>();
    }
    
    private static void AddHangfireJobs(this IServiceCollection services)
    {
        services.AddScoped<Processors.OutboxProcessor>();
        services.AddScoped<OutboxJob>();
        
        services.AddHangfire(config =>
        {
            config.UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseMemoryStorage(); // TODO: заменить storage на redis + прикрутить логирование
        });
        
        services.AddHangfireServer();
    }
}