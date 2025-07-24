using NotifyHub.TelegramBot.Domain.DTOs;

namespace NotifyHub.TelegramBot.Infrastructure.Managers;

public class NotificationCreationSessionManager
{
    private readonly Dictionary<long, CreateNotificationDraft> _drafts = new();

    public bool HasDraft(long userId) => _drafts.ContainsKey(userId);
    
    public CreateNotificationDraft GetOrCreateDraft(long userId)
    {
        if (!_drafts.ContainsKey(userId))
            _drafts[userId] = new CreateNotificationDraft();
        return _drafts[userId];
    }

    public void RemoveDraft(long userId) => _drafts.Remove(userId);
}
