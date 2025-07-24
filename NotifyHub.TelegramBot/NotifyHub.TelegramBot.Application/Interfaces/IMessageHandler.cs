using NotifyHub.TelegramBot.Domain.Common.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace NotifyHub.TelegramBot.Application.Interfaces;

public interface IMessageHandler
{
    bool CanHandle(Message msg, UserState state);
    
    Task HandleAsync(Message msg, ITelegramBotClient bot, CancellationToken token);
}
