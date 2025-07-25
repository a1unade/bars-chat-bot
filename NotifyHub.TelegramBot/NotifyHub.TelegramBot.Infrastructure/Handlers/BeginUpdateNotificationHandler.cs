using NotifyHub.TelegramBot.Application.Interfaces;
using NotifyHub.TelegramBot.Domain.Common.Enums;
using NotifyHub.TelegramBot.Infrastructure.Managers;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace NotifyHub.TelegramBot.Infrastructure.Handlers;

public class BeginUpdateNotificationHandler(
    INotificationService notificationService,
    NotificationUpdateSessionManager sessionManager,
    IUserStateService userStateService)
    : IMessageHandler
{
    public bool CanHandle(Message msg, UserState state) =>
        msg.Text?.Trim().ToLower() == "изменить";

    public async Task HandleAsync(Message msg, ITelegramBotClient bot, CancellationToken token)
    {
        var userId = msg.From!.Id;
        var notifications = await notificationService.GetUserNotificationsAsync(userId, token);

        if (notifications.Count == 0)
        {
            await bot.SendMessage(msg.Chat.Id, "У тебя нет уведомлений для обновления.", cancellationToken: token);
            return;
        }

        sessionManager.StartSession(userId, notifications.Select(n => n.Id).ToList());
        userStateService.SetState(userId, UserState.UpdatingNotification);

        var listText = string.Join("\n", notifications.Select((n, i) =>
            $"{i + 1}. {n.Title} ({n.ScheduledAt:dd.MM.yyyy})"));
        
        var cancelMarkup = new ReplyKeyboardMarkup([
            ["Отмена"]
        ])
        {
            ResizeKeyboard = true,
            OneTimeKeyboard = false
        };

        await bot.SendMessage(msg.Chat.Id,
            $"Что обновить?\n\n{listText}\n\nВведи номер уведомления для редактирования.",
            replyMarkup: cancelMarkup,
            cancellationToken: token);
    }
}
