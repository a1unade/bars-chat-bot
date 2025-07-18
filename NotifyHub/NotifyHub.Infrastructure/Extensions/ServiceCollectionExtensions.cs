using System.Reflection;
using NotifyHub.Application.Interfaces;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotifyHub.Application.Features.Queries;

namespace NotifyHub.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructureLayer(this IServiceCollection services)
    {
        services.AddGraphQl();
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