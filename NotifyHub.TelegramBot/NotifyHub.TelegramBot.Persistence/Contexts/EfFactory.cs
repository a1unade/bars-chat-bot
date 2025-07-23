using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace NotifyHub.TelegramBot.Persistence.Contexts;

public class EfFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        
        // TODO: поправить на подстановку строки подключения из appsettings.json
        // var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__Postgres");
        
        optionBuilder.UseNpgsql("Host=telegram_db;Port=5432;Username=postgres;Password=notify_hub_telegram;Database=notify_hub_telegram_db");
        
        return new ApplicationDbContext(optionBuilder.Options);
    }
}