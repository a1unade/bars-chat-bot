using NotifyHub.TelegramBot.Application.Interfaces;
using NotifyHub.TelegramBot.Domain.Common.Enums;
using NotifyHub.TelegramBot.Infrastructure.Managers;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace NotifyHub.TelegramBot.Infrastructure.Handlers;

public class BeginCreateNotificationHandler(
    NotificationCreationSessionManager sessionManager,
    IUserStateService userStateService)
    : IMessageHandler
{
    public bool CanHandle(Message msg, UserState state)
    {
        return state == UserState.Default &&
               msg.Text?.Trim().ToLower() == "создать";
    }

    public async Task HandleAsync(Message msg, ITelegramBotClient bot, CancellationToken token)
    {
        var userId = msg.From!.Id;

        sessionManager.GetOrCreateDraft(userId);
        userStateService.SetState(userId, UserState.CreatingNotification);

        var cancelMarkup = new ReplyKeyboardMarkup([
            ["Отмена"]
        ])
        {
            ResizeKeyboard = true,
            OneTimeKeyboard = false
        };

        await bot.SendMessage(
            chatId: msg.Chat.Id,
            text: "Введите заголовок уведомления:",
            replyMarkup: cancelMarkup,
            cancellationToken: token
        );
    }
}