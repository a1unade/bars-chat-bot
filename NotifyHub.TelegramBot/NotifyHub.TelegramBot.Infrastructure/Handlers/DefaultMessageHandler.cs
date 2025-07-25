using NotifyHub.TelegramBot.Application.Interfaces;
using NotifyHub.TelegramBot.Domain.Common.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace NotifyHub.TelegramBot.Infrastructure.Handlers;

public class DefaultMessageHandler : IMessageHandler
{
    public bool CanHandle(Message msg, UserState state)
    {
        // Обрабатывает всё, что не обработали другие хендлеры
        
        var text = msg.Text ?? string.Empty;

        return !text.StartsWith("/start")
               && !text.StartsWith("/notifications")
               && !text.StartsWith("/help");
    }

    public async Task HandleAsync(Message msg, ITelegramBotClient bot, CancellationToken token)
    {
        var keyboard = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[] { "Получить уведомления", "Помощь" }
        })
        {
            ResizeKeyboard = true,
            OneTimeKeyboard = false
        };
        
        await bot.SendMessage(
            chatId: msg.Chat.Id,
            text: "Команда не распознана. Используй /help для списка команд.",
            replyMarkup: keyboard,
            cancellationToken: token);
    }
}