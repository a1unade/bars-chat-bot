using NotifyHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using NotifyHub.Application.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace NotifyHub.Persistence.Contexts;

public class ApplicationDbContext: DbContext, IDbContext
{
    public DbSet<User> Users { get; set; }
    
    public DbSet<Notification> Notifications { get; set; }
    
    public DbSet<NotificationLog> NotificationLogs { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Extensions.ServiceCollectionExtensions).Assembly);
        base.OnModelCreating(modelBuilder);
    }
    
    public IDbContextTransaction? CurrentTransaction
        => Database.CurrentTransaction;
    
    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default) 
        => await Database.BeginTransactionAsync(cancellationToken);
}