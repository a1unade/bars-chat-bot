using NotifyHub.Application.Interfaces;
using NotifyHub.Application.Interfaces.Repositories;

namespace NotifyHub.Persistence.Repositories;

public class NotificationLogRepository(IDbContext context): INotificationLogRepository
{
    private readonly IDbContext _context = context;
    
    // TODO реализовать CRUD-взаимодействие с бд через IDbContext и EntityFramework
}