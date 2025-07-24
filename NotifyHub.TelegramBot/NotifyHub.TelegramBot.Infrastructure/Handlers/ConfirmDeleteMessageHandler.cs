using NotifyHub.TelegramBot.Application.Interfaces;
using NotifyHub.TelegramBot.Domain.Common.Enums;
using NotifyHub.TelegramBot.Infrastructure.Managers;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace NotifyHub.TelegramBot.Infrastructure.Handlers;

public class ConfirmDeleteMessageHandler: IMessageHandler
{
    private readonly INotificationService _notificationService;
    private readonly NotificationDeletionSessionManager _sessionManager;

    public ConfirmDeleteMessageHandler(
        INotificationService notificationService,
        NotificationDeletionSessionManager sessionManager)
    {
        _notificationService = notificationService;
        _sessionManager = sessionManager;
    }

    public bool CanHandle(Message msg, UserState state)
    {
        return _sessionManager.HasSession(msg.From!.Id) &&
               int.TryParse(msg.Text, out _);
    }

    public async Task HandleAsync(Message msg, ITelegramBotClient bot, CancellationToken token)
    {
        var userId = msg.From!.Id;
        var notifications = _sessionManager.GetNotifications(userId);

        if (!int.TryParse(msg.Text, out var index) || index < 1 || index > notifications.Count)
        {
            await bot.SendMessage(msg.Chat, "Неверный номер. Попробуй снова.", cancellationToken: token);
            return;
        }

        var notificationId = notifications[index - 1];

        await _notificationService.DeleteNotificationAsync(notificationId, token);
        _sessionManager.EndSession(userId);

        await bot.SendMessage(msg.Chat, "Уведомление удалено.", cancellationToken: token);
    }
}