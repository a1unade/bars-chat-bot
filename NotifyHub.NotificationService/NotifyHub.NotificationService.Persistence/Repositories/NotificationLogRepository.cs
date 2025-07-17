using NotifyHub.NotificationService.Application.Interfaces;

namespace NotifyHub.NotificationService.Persistence.Repositories;

public class NotificationLogRepository(IDbContext context): INotificationLogRepository
{
    private readonly IDbContext _context = context;
    
    // TODO реализовать CRUD-взаимодействие с бд через IDbContext и EntityFramework
}