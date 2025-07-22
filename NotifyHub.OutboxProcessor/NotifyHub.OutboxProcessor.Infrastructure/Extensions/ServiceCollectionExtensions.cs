using Hangfire;
using Hangfire.Redis.StackExchange;
using NotifyHub.Kafka.Extensions;
using NotifyHub.Abstractions.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotifyHub.OutboxProcessor.Infrastructure.Jobs;
using NotifyHub.OutboxProcessor.Infrastructure.Workers;
using StackExchange.Redis;

namespace NotifyHub.OutboxProcessor.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRedis(configuration);
        services.AddHangfireJobs(configuration);
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
    
    private static void AddHangfireJobs(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<Processors.OutboxProcessor>();
        services.AddScoped<OutboxJob>();
        
        var redisConnection = configuration.GetConnectionString("Redis");
        
        services.AddHangfire(config =>
        {
            config
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseRedisStorage(redisConnection, new RedisStorageOptions
                {
                    Db = 5,
                    Prefix = "hangfire:"
                });
            
            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute
            {
                Attempts = 5,
                DelaysInSeconds = [60, 120, 300],
                OnAttemptsExceeded = AttemptsExceededAction.Fail
            });
        });
        
        services.AddHangfireServer(options =>
        {
            options.WorkerCount = Environment.ProcessorCount * 2;
        });
    }

    private static void AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConnectionMultiplexer>(_ =>
            ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")!));
    }
}