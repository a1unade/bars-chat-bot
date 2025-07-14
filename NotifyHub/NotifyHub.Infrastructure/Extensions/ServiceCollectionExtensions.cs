using NotifyHub.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using NotifyHub.Application.Interfaces.Services;

namespace NotifyHub.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructureLayer(this IServiceCollection services)
    {
        services.AddServices();
        services.AddGraphQl();
    }
    
    private static void AddServices(this IServiceCollection services)
    {
        services
            .AddScoped<IEmailService, EmailService>();
    }

    private static void AddGraphQl(this IServiceCollection services)
    {
        // TODO: реализовать query и mutation для graphql в Application слое
        services
            .AddGraphQLServer()
            .AddQueryType(d => d.Name("Query"))
            .AddMutationType(d => d.Name("Mutation"))
            .AddFiltering()
            .AddSorting();
    }
}