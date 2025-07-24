namespace NotifyHub.TelegramBot.Infrastructure.Managers;

public class NotificationDeletionSessionManager
{
    private readonly Dictionary<long, List<Guid>> _deletionTargets = new();

    public void StartSession(long userId, List<Guid> notificationIds)
        => _deletionTargets[userId] = notificationIds;

    public bool HasSession(long userId) => _deletionTargets.ContainsKey(userId);

    public List<Guid> GetNotifications(long userId) => _deletionTargets[userId];

    public void EndSession(long userId) => _deletionTargets.Remove(userId);
}