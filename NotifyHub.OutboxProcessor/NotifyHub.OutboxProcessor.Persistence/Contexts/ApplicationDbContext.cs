using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NotifyHub.OutboxProcessor.Application.Interfaces;
using NotifyHub.OutboxProcessor.Domain.Entities;

namespace NotifyHub.OutboxProcessor.Persistence.Contexts;

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