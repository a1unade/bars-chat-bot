using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotifyHub.TelegramBot.Application.Interfaces;
using NotifyHub.TelegramBot.Persistence.Contexts;
using NotifyHub.TelegramBot.Persistence.MigrationTools;
using NotifyHub.TelegramBot.Persistence.Repositories;

namespace NotifyHub.TelegramBot.Persistence.Extensions;

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
            .AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
    }
}