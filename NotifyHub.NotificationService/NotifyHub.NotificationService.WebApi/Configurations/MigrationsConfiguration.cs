using NotifyHub.NotificationService.Persistence.MigrationTools;

namespace NotifyHub.NotificationService.WebApi.Configurations;

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