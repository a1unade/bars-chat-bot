using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NotifyHub.NotificationService.Application.Interfaces;
using NotifyHub.NotificationService.Domain.Entities;

namespace NotifyHub.NotificationService.Persistence.Contexts;

public class ApplicationDbContext: DbContext, IDbContext
{
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    
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