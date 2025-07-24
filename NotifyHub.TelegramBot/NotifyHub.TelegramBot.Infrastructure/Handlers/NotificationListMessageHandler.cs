using NotifyHub.TelegramBot.Application.Interfaces;
using NotifyHub.TelegramBot.Domain.Common.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace NotifyHub.TelegramBot.Infrastructure.Handlers;

public class NotificationListMessageHandler : IMessageHandler
{
    private readonly INotificationService _notificationService;

    public NotificationListMessageHandler(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public bool CanHandle(Message msg, UserState state)
        => msg.Text == "Получить уведомления";

    public async Task HandleAsync(Message msg, ITelegramBotClient bot, CancellationToken token)
    {
        var notifications = await _notificationService.GetUserNotificationsAsync(msg.From!.Id, token);

        if (notifications.Count == 0)
        {
            await bot.SendMessage(msg.Chat, "У тебя нет уведомлений.", cancellationToken: token);
            return;
        }

        var cards = notifications.Select((n, i) =>
            $"<b>{i + 1}. {Escape(n.Title)}</b>\n" +
            $"<i>{Escape(n.Description)}</i>\n" +
            $"<b>Дата:</b> {n.ScheduledAt:dd.MM.yyyy HH:mm}\n" +
            $"<b>Тип:</b> {FormatType(n.Type, n.Frequency)}");

        var message = "<b>Твои уведомления:</b>\n\n" + string.Join("\n\n", cards);
        
        var keyboard = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[] { "Создать", "Удалить", "Изменить" }
        })
        {
            ResizeKeyboard = true,
            OneTimeKeyboard = false
        };

        await bot.SendMessage(
            chatId: msg.Chat.Id,
            text: message,
            parseMode: Telegram.Bot.Types.Enums.ParseMode.Html,
            replyMarkup: keyboard,
            cancellationToken: token
        );
    }
    
    private string FormatType(string type, string? frequency)
    {
        return type switch
        {
            "ONE_TIME" => "Одноразовое",
            "RECURRING" => $"Периодичное ({frequency})",
            _ => type
        };
    }

    private string Escape(string input)
    {
        return input
            .Replace("&", "&amp;")
            .Replace("<", "&lt;")
            .Replace(">", "&gt;");
    }
}