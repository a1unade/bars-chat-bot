using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NotifyHub.Application.Interfaces;

namespace NotifyHub.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationLayer(this IServiceCollection services)
    {
        services.AddQueries();
    }

    private static void AddQueries(this IServiceCollection services)
    {
        var queryTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => !t.IsAbstract && typeof(IQuery).IsAssignableFrom(t));

        foreach (var type in queryTypes)
            services.AddSingleton(typeof(IQuery), type);
    }
    
    // TODO: зарегистрировать mutations для graphql
}