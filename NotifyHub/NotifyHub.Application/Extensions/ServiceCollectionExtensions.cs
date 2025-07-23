using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using NotifyHub.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace NotifyHub.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationLayer(this IServiceCollection services)
    {
        services.AddQueries();
        services.AddMutation();
        services.AddMediator();
        services.AddAutoMapper();
        services.AddValidators();
    }
    
    private static void AddMediator(this IServiceCollection services) =>
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

    private static void AddAutoMapper(this IServiceCollection services) => 
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    
    private static void AddValidators(this IServiceCollection services) =>
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

    private static void AddQueries(this IServiceCollection services)
    {
        var queryTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => !t.IsAbstract && typeof(IQuery).IsAssignableFrom(t));

        foreach (var type in queryTypes)
            services.AddSingleton(type);
    }
    
    private static void AddMutation(this IServiceCollection services)
    {
        var queryTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => !t.IsAbstract && typeof(IMutation).IsAssignableFrom(t));

        foreach (var type in queryTypes)
            services.AddSingleton(type);
    }
}