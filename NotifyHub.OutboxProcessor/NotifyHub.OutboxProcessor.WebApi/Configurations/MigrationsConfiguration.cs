using NotifyHub.OutboxProcessor.Persistence.MigrationTools;

namespace NotifyHub.OutboxProcessor.WebApi.Configurations;

public static class MigrationsConfiguration
{
    public static async Task UseMigrations(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var migrator = scope.ServiceProvider.GetRequiredService<Migrator>();

            await migrator.MigrateAsync();
        }
    }
}