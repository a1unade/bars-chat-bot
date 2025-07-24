using NotifyHub.TelegramBot.Application.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace NotifyHub.TelegramBot.Infrastructure.Handlers;

public class NotificationCallbackHandler : ICallbackHandler
{
    public bool CanHandle(string callbackData)
    {
        // Здесь проверяешь, обрабатывает ли этот хендлер данный callback
        return callbackData.StartsWith("notifications-");
    }

    public async Task HandleAsync(CallbackQuery query, ITelegramBotClient bot, CancellationToken cancellationToken)
    {
        if (query.Data == "notifications-list")
        {
            await bot.SendMessage(query.Message!.Chat.Id, "Список уведомлений", cancellationToken: cancellationToken);
        }
        else if (query.Data == "notifications-delete")
        {
            await bot.SendMessage(query.Message!.Chat.Id, "Удаление уведомлений", cancellationToken: cancellationToken);
        }
        else
        {
            await bot.SendMessage(query.Message!.Chat.Id, "Неизвестная команда", cancellationToken: cancellationToken);
        }

        await bot.AnswerCallbackQuery(query.Id, cancellationToken: cancellationToken);
    }
}