using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NotifyHub.Persistence.Contexts;

namespace NotifyHub.Persistence.MigrationTools;

public class Migrator(ApplicationDbContext context, ILogger<Migrator> logger)
{
    public async Task MigrateAsync()
    {
        try
        {
            await context.Database.MigrateAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError($"migrations apply failed { 0 }", ex.Message);
            throw;
        }
    }
}