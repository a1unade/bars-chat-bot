using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace NotifyHub.OutboxProcessor.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationLayer(this IServiceCollection services)
    {
        services.AddAutoMapper();
        services.AddMediator();
    }

    private static void AddAutoMapper(this IServiceCollection services) => 
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    
    private static void AddMediator(this IServiceCollection services) =>
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
}