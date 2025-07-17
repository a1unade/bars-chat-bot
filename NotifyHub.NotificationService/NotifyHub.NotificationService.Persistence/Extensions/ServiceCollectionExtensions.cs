using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotifyHub.NotificationService.Application.Interfaces;
using NotifyHub.NotificationService.Domain.Entities;
using NotifyHub.NotificationService.Persistence.Contexts;
using NotifyHub.NotificationService.Persistence.MigrationTools;
using NotifyHub.NotificationService.Persistence.Repositories;

namespace NotifyHub.NotificationService.Persistence.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext(configuration);
        services.AddContextAndRepositories();
    }
    
    private static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Postgres");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString,
                builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
    }
    
    private static void AddContextAndRepositories(this IServiceCollection services)
    {
        services
            .AddTransient<Migrator>()
            .AddScoped<IDbContext, ApplicationDbContext>()
            .AddScoped<INotificationLogRepository, NotificationLogRepository>();
    }
}