using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotifyHub.Application.Interfaces;
using NotifyHub.Application.Interfaces.Repositories;
using NotifyHub.Persistence.Contexts;
using NotifyHub.Persistence.MigrationTools;
using NotifyHub.Persistence.Repositories;

namespace NotifyHub.Persistence.Extensions;

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
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<INotificationRepository, NotificationRepository>()
            .AddScoped<IOutboxMessageRepository, OutboxMessageRepository>()
            .AddScoped<INotificationLogRepository, NotificationLogRepository>();
    }
}