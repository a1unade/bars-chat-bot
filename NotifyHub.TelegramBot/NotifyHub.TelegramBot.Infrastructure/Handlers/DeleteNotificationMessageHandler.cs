using NotifyHub.TelegramBot.Application.Interfaces;
using NotifyHub.TelegramBot.Domain.Common.Enums;
using NotifyHub.TelegramBot.Infrastructure.Managers;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace NotifyHub.TelegramBot.Infrastructure.Handlers;

public class DeleteNotificationMessageHandler : IMessageHandler
{
    private readonly INotificationService _notificationService;
    private readonly NotificationDeletionSessionManager _sessionManager;
    private readonly IUserStateService _userStateService;

    public DeleteNotificationMessageHandler(
        INotificationService notificationService,
        NotificationDeletionSessionManager sessionManager,
        IUserStateService userStateService)
    {
        _notificationService = notificationService;
        _sessionManager = sessionManager;
        _userStateService = userStateService;
    }

    public bool CanHandle(Message msg, UserState state)
    {
        return msg.Text?.Trim().ToLower() == "удалить";
    }

    public async Task HandleAsync(Message msg, ITelegramBotClient bot, CancellationToken token)
    {
        var notifications = await _notificationService.GetUserNotificationsAsync(msg.From!.Id, token);

        if (notifications.Count == 0)
        {
            await bot.SendMessage(msg.Chat, "У тебя нет уведомлений для удаления.", cancellationToken: token);
            return;
        }

        var userId = msg.From!.Id;
        _sessionManager.StartSession(msg.From.Id, notifications.Select(n => n.Id).ToList());
        _userStateService.SetState(userId, UserState.DeletingNotification);

        var listText = string.Join("\n", notifications.Select((n, i) =>
            $"{i + 1}. {n.Title} ({n.ScheduledAt:dd.MM.yyyy})"));

        await bot.SendMessage(msg.Chat,
            $"Что удалить?\n\n{listText}\n\nВведите номер уведомления для удаления.",
            cancellationToken: token);
    }
}