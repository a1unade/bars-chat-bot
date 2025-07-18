using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.Extensions.DependencyInjection;
using NotifyHub.OutboxProcessor.Infrastructure.Jobs;

namespace NotifyHub.OutboxProcessor.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructureLayer(this IServiceCollection services)
    {
        services.AddHangfireJobs();
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