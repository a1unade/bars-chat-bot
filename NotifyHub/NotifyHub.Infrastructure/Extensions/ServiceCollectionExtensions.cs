using System.Reflection;
using Hangfire;
using Hangfire.MemoryStorage;
using NotifyHub.Application.Interfaces;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotifyHub.Application.Features.Queries;
using NotifyHub.Infrastructure.Jobs;
using NotifyHub.Infrastructure.Processors;

namespace NotifyHub.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructureLayer(this IServiceCollection services)
    {
        services.AddGraphQl();
        services.AddHangfireJobs();
    }

    private static void AddHangfireJobs(this IServiceCollection services)
    {
        services.AddScoped<OutboxProcessor>();
        services.AddScoped<OutboxJob>();
        
        services.AddHangfire(config =>
        {
            config.UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseMemoryStorage(); // TODO: заменить storage на redis + прикрутить логирование
        });
        
        services.AddHangfireServer();
    }

    private static void AddGraphQl(this IServiceCollection services)
    {
        // TODO: реализовать query и mutation для graphql в Application слое
        services
            .AddGraphQLServer()
            .AddQueryType(d => d.Name("Query"))
            .AddTypeExtensionsFromAssembly(typeof(BaseQuery).Assembly)
            .AddFiltering()
            .AddSorting();
    }
    
    /// <summary>
    /// Extension-метод для регистрации всех Query через сборку, 
    /// помеченных атрибутом <see cref="ExtendObjectTypeAttribute"/> и реализующих <see cref="IQuery"/>
    /// </summary>
    /// <param name="builder">GraphQL-билдер HotChocolate</param>
    /// <param name="assembly">Сборка, в которой будут найдены и зарегистрированы все Query</param>
    private static IRequestExecutorBuilder AddTypeExtensionsFromAssembly(
        this IRequestExecutorBuilder builder,
        Assembly assembly)
    {
        var queryTypes = assembly.GetTypes()
            .Where(t => !t.IsAbstract && typeof(IQuery).IsAssignableFrom(t));

        foreach (var type in queryTypes)
            builder.AddTypeExtension(type);

        return builder;
    }
}