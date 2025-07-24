using Telegram.Bot;
using Telegram.Bot.Types;

namespace NotifyHub.TelegramBot.Application.Interfaces;

public interface ICallbackHandler
{
    bool CanHandle(string callbackData);
    
    Task HandleAsync(CallbackQuery query, ITelegramBotClient bot, CancellationToken ct);
}