using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NotifyHub.TelegramBot.Persistence.Contexts;

namespace NotifyHub.TelegramBot.Persistence.MigrationTools;

public class Migrator(ApplicationDbContext context, ILogger<Migrator> logger)
{
    public async Task MigrateAsync()
    {
        try
        {
            await context.Database.MigrateAsync().ConfigureAwait(false);
            
            logger.LogInformation("Migrations applied");
        }
        catch (Exception ex)
        {
            logger.LogError("Migrations apply failed {0}", ex.Message);
            throw;
        }
    }
}