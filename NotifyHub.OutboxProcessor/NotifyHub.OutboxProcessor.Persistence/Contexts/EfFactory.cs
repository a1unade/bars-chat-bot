using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace NotifyHub.OutboxProcessor.Persistence.Contexts;

public class EfFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        
        // TODO: поправить на подстановку строки подключения из appsettings.json
        // var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__Postgres");
        
        optionBuilder.UseNpgsql("Host=outbox_db;Port=5432;Username=postgres;Password=notify_hub_outbox;Database=notify_hub_outbox_db");
        
        return new ApplicationDbContext(optionBuilder.Options);
    }
}