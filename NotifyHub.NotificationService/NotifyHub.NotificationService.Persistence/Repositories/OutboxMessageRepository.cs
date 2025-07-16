using NotifyHub.NotificationService.Application.Interfaces;

namespace NotifyHub.NotificationService.Persistence.Repositories;

public class OutboxMessageRepository(IDbContext context): IOutboxMessageRepository
{
    private readonly IDbContext _context = context;
    
    // TODO реализовать CRUD-взаимодействие с бд через IDbContext и EntityFramework
}