using Microsoft.Extensions.Logging;
using NotifyHub.Application.Interfaces;
using NotifyHub.Application.Interfaces.Repositories;
using NotifyHub.Domain.Common.Enums;
using NotifyHub.Domain.Entities;

namespace NotifyHub.Persistence.Seeder;

public class DbSeeder(
    IDbContext context, 
    ILogger<IDbSeeder> logger, 
    IGenericRepository<User> userRepository,
    IGenericRepository<Notification> notificationRepository,
    IGenericRepository<OutboxMessage> outboxRepository
    ): IDbSeeder
{
    private readonly IDbContext _context = context;
    private readonly ILogger<IDbSeeder> _logger = logger;
    private readonly IGenericRepository<User> _userRepository = userRepository;
    private readonly IGenericRepository<Notification> _notificationRepository = notificationRepository;
    private readonly IGenericRepository<OutboxMessage> _outboxRepository = outboxRepository;
    
    private static User _user = new User
    {
        Id = Guid.NewGuid(),
        Email = "example@example.com",
        Name = "Example"
    };
    
    private static ICollection<Notification> _notifications = new List<Notification>
    {
        new()
        {
            Id = Guid.NewGuid(),
            Title = "notification 1",
            Description = "description",
            Type = NotificationType.OneTime,
            Frequency = null,
            ScheduledAt = DateTime.UtcNow.AddMinutes(1),
            UserId = _user.Id,
            User = _user
        },
        new()
        {
            Id = Guid.NewGuid(),
            Title = "notification 2",
            Description = "description",
            Type = NotificationType.Recurring,
            Frequency = NotificationFrequency.Daily,
            ScheduledAt = DateTime.UtcNow.AddMinutes(5),
            UserId = _user.Id,
            User = _user
        }
    };
    
    private static ICollection<OutboxMessage> _outboxMessages = new List<OutboxMessage>
    {
        new()
        {
            Id = Guid.NewGuid(),
            NotificationId = _notifications.ElementAt(0).Id,
            Notification = _notifications.ElementAt(0),
            ScheduledAt = _notifications.ElementAt(0).ScheduledAt,
            SentAt = null,
            Status = OperationStatus.Created,
            Error = null,
            PayloadJson = "{\"message\":\"Notification 1 payload\"}"
        },
        new()
        {
            Id = Guid.NewGuid(),
            NotificationId = _notifications.ElementAt(1).Id,
            Notification = _notifications.ElementAt(1),
            ScheduledAt = _notifications.ElementAt(1).ScheduledAt,
            SentAt = null,
            Status = OperationStatus.Created,
            Error = null,
            PayloadJson = "{\"message\":\"Notification 2 payload\"}"
        }
    };
    
    private async Task SeedUserAsync(CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetByIdAsync(_user.Id, cancellationToken);

        if (existingUser is null)
            await _userRepository.AddAsync(_user, cancellationToken);
    }

    private async Task SeedNotificationsAsync(CancellationToken cancellationToken)
    {
        foreach (var notification in _notifications)
        {
            var existingNotification = _notificationRepository
                .Get(n => n.Id == notification.Id)
                .AsQueryable()
                .FirstOrDefault();

            if (existingNotification is null)
                await _notificationRepository.AddAsync(notification, cancellationToken);
        }
    }

    private async Task SeedOutboxMessagesAsync(CancellationToken cancellationToken)
    {
        foreach (var outboxMessage in _outboxMessages)
        {
            var existingMessage = _outboxRepository
                .Get(n => n.Id == outboxMessage.Id)
                .AsQueryable()
                .FirstOrDefault();

            if (existingMessage is null)
                await _outboxRepository.AddAsync(outboxMessage, cancellationToken);
        }
    }
        
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        await SeedUserAsync(cancellationToken);
        await SeedNotificationsAsync(cancellationToken);
        await SeedOutboxMessagesAsync(cancellationToken);
        
        await _context.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Seeds applied");
    }
}