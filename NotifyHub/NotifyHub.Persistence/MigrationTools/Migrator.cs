using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NotifyHub.Application.Interfaces;
using NotifyHub.Persistence.Contexts;

namespace NotifyHub.Persistence.MigrationTools;

public class Migrator(ApplicationDbContext context, IDbSeeder seeder, ILogger<Migrator> logger)
{
    public async Task MigrateAsync()
    {
        try
        {
            await context.Database.MigrateAsync().ConfigureAwait(false);
            await seeder.SeedAsync();
            
            logger.LogInformation("Migrations and seeds applied");
        }
        catch (Exception ex)
        {
            logger.LogError("Migrations and seeds apply failed {0}", ex.Message);
            throw;
        }
    }
}