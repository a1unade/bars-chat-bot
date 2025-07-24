using NotifyHub.TelegramBot.Application.Interfaces;
using NotifyHub.TelegramBot.Domain.Common.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace NotifyHub.TelegramBot.Infrastructure.Handlers;

public class HelpMessageHandler : IMessageHandler
{
    public bool CanHandle(Message msg, UserState state)
        => msg.Text == "Помощь";

    public async Task HandleAsync(Message msg, ITelegramBotClient bot, CancellationToken token)
    {
        await bot.SendMessage(msg.Chat, "ℹ️ Помощь:\n\nДоступные команды:\n/start — регистрация\n/notifications — список уведомлений\n/help — помощь", cancellationToken: token);
    }
}