using System.Reflection;
using NotifyHub.Application.Interfaces;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotifyHub.Application.Features.Queries;
using NotifyHub.Domain.DTOs;
using NotifyHub.Infrastructure.Configurations;
using NotifyHub.Kafka.Extensions;

namespace NotifyHub.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddGraphQl();
        services.AddKafka(configuration);
    }

    public static void AddKafka(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddKafkaProducer<NotificationEventDto>(configuration);
    }

    private static void AddGraphQl(this IServiceCollection services)
    {
        // TODO: реализовать query и mutation для graphql в Application слое
        services
            .AddGraphQLServer()
            .AddQueryType(d => d.Name("Query"))
            .AddMutationType(d => d.Name("Mutation"))
            .AddTypeExtensionsFromAssembly(typeof(BaseQuery).Assembly)
            .AddFiltering()
            .AddSorting()
            .AddErrorFilter<GraphQLErrorFilter>();
    }
    
    /// <summary>
    /// Extension-метод для регистрации всех Query и Mutation через сборку, 
    /// помеченных атрибутом <see cref="ExtendObjectTypeAttribute"/> и реализующих <see cref="IQuery"/> и <see cref="IMutation"/>
    /// </summary>
    /// <param name="builder">GraphQL-builder HotChocolate</param>
    /// <param name="assembly">Сборка, в которой будут найдены и зарегистрированы все Query и Mutation</param>
    private static IRequestExecutorBuilder AddTypeExtensionsFromAssembly(
        this IRequestExecutorBuilder builder,
        Assembly assembly)
    {
        var queryTypes = assembly.GetTypes()
            .Where(t => !t.IsAbstract && typeof(IQuery).IsAssignableFrom(t));
        
        var mutationTypes = assembly.GetTypes()
            .Where(t => !t.IsAbstract && typeof(IMutation).IsAssignableFrom(t));

        foreach (var type in queryTypes)
            builder.AddTypeExtension(type);
        
        foreach (var type in mutationTypes)
            builder.AddTypeExtension(type);

        return builder;
    }
}