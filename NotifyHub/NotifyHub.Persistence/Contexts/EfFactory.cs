using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace NotifyHub.Persistence.Contexts;

public class EfFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        
        // TODO: поправить на подстановку строки подключения из appsettings.json
        // var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__Postgres");
        
        optionBuilder.UseNpgsql("Host=localhost;Port=5432;Username=postgres;Password=notify_hub;Database=notify_hub_db");
        
        return new ApplicationDbContext(optionBuilder.Options);
    }
}