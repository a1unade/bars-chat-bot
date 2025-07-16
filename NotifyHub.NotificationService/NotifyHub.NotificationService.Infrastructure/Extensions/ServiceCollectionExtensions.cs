using Microsoft.Extensions.DependencyInjection;
using NotifyHub.NotificationService.Application.Interfaces;
using NotifyHub.NotificationService.Infrastructure.Services;

namespace NotifyHub.NotificationService.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructureLayer(this IServiceCollection services)
    {
        services.AddServices();
    }
    
    private static void AddServices(this IServiceCollection services)
    {
        services
            .AddScoped<IEmailService, EmailService>();
    }
}