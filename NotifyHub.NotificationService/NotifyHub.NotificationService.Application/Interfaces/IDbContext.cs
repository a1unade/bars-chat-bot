using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NotifyHub.NotificationService.Domain.Entities;

namespace NotifyHub.NotificationService.Application.Interfaces;

public interface IDbContext
{
    /// <summary>
    /// Операции по отправке
    /// </summary>
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    
    /// <summary>
    /// Универсальный доступ к DbSet по типу сущности
    /// </summary>
    DbSet<T> Set<T>() where T : class;
    
    /// <summary>
    /// Получить текущую транзакцию
    /// </summary>
    IDbContextTransaction? CurrentTransaction { get; }
    
    /// <summary>
    /// Начать транзакцию
    /// </summary>
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Сохранение изменений
    /// </summary>
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}