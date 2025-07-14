using NotifyHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace NotifyHub.Application.Interfaces;

public interface IDbContext
{
    /// <summary>
    /// Пользователи
    /// </summary>
    public DbSet<User> Users { get; set; }
    
    /// <summary>
    /// Уведомления
    /// </summary>
    public DbSet<Notification> Notifications { get; set; }
    
    /// <summary>
    /// Операции по отправке
    /// </summary>
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    
    /// <summary>
    /// История 
    /// </summary>
    public DbSet<NotificationLog> NotificationLogs { get; set; }
    
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