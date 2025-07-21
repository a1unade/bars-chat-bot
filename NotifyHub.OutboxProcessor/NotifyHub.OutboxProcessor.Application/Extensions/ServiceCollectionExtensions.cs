using Microsoft.Extensions.DependencyInjection;

namespace NotifyHub.OutboxProcessor.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationLayer(this IServiceCollection services)
    {
        services.AddAutoMapper();
    }

    private static void AddAutoMapper(this IServiceCollection services) => 
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
}