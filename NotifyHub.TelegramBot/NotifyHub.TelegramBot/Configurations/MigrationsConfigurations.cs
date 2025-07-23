using NotifyHub.TelegramBot.Persistence.MigrationTools;

namespace NotifyHub.TelegramBot.Configurations;

public static class MigrationsConfigurations
{
    public static async Task UseMigrationsAsync(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var migrator = scope.ServiceProvider.GetRequiredService<Migrator>();

        await migrator.MigrateAsync();
    }
}