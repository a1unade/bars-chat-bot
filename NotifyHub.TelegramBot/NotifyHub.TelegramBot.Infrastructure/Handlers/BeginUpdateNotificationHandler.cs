using NotifyHub.TelegramBot.Application.Interfaces;
using NotifyHub.TelegramBot.Domain.Common.Enums;
using NotifyHub.TelegramBot.Infrastructure.Managers;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace NotifyHub.TelegramBot.Infrastructure.Handlers;

public class BeginUpdateNotificationHandler(
    NotificationUpdateSessionManager sessionManager,
    IUserStateService userStateService)
    : IMessageHandler
{
    public bool CanHandle(Message msg, UserState state)
    {
        return state == UserState.Default &&
               msg.Text?.Trim().ToLower() == "обновить";
    }

    public async Task HandleAsync(Message msg, ITelegramBotClient bot, CancellationToken token)
    {
        var userId = msg.From!.Id;
        sessionManager.GetOrCreateDraft(userId);
        userStateService.SetState(userId, UserState.UpdatingNotification);

        await bot.SendMessage(userId, "Введите ID уведомления для обновления:", cancellationToken: token);
    }
}