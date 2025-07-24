using NotifyHub.TelegramBot.Domain.DTOs;

namespace NotifyHub.TelegramBot.Infrastructure.Managers;

public class NotificationUpdateSessionManager
{
    private readonly Dictionary<long, UpdateNotificationDraft> _drafts = new();

    public bool HasDraft(long userId) => _drafts.ContainsKey(userId);

    public UpdateNotificationDraft GetOrCreateDraft(long userId)
    {
        if (!_drafts.ContainsKey(userId))
            _drafts[userId] = new UpdateNotificationDraft();
        return _drafts[userId];
    }

    public UpdateNotificationDraft? GetDraft(long userId) =>
        _drafts.GetValueOrDefault(userId);

    public void RemoveDraft(long userId) => _drafts.Remove(userId);
}