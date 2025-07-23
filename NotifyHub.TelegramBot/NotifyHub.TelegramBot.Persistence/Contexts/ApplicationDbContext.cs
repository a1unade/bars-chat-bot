using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NotifyHub.TelegramBot.Application.Interfaces;
using NotifyHub.TelegramBot.Domain.Entities;

namespace NotifyHub.TelegramBot.Persistence.Contexts;

public class ApplicationDbContext: DbContext, IDbContext
{
    public DbSet<TelegramUser> TelegramUsers { get; set; }
    
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