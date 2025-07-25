using NotifyHub.TelegramBot.Domain.DTOs;

namespace NotifyHub.TelegramBot.Infrastructure.Managers;

public class NotificationUpdateSessionManager
{
    private readonly Dictionary<long, (List<Guid> Ids, Guid? SelectedId, UpdateNotificationDraft Draft)> _sessions = new();

    public void StartSession(long userId, List<Guid> ids)
    {
        _sessions[userId] = (ids, null, new UpdateNotificationDraft());
    }

    public bool HasSession(long userId) => _sessions.ContainsKey(userId);

    public void SetSelected(long userId, int index)
    {
        var session = _sessions[userId];
        session.SelectedId = session.Ids[index];
        _sessions[userId] = session;
    }

    public Guid GetSelectedId(long userId)
    {
        if (!_sessions.TryGetValue(userId, out var session) || session.SelectedId is null)
            return Guid.Empty;

        return session.SelectedId.Value;
    }

    public bool HasSelectedId(long userId)
    {
        return _sessions.TryGetValue(userId, out var session) && session.SelectedId != null;
    }

    public UpdateNotificationDraft GetDraft(long userId) => _sessions[userId].Draft;

    public void RemoveSession(long userId) => _sessions.Remove(userId);
}