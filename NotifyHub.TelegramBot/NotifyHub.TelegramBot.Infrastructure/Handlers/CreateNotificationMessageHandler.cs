using NotifyHub.TelegramBot.Application.Interfaces;
using NotifyHub.TelegramBot.Domain.Common.Enums;
using NotifyHub.TelegramBot.Infrastructure.Managers;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace NotifyHub.TelegramBot.Infrastructure.Handlers;

public class CreateNotificationMessageHandler : IMessageHandler
{
    private readonly NotificationCreationSessionManager _session;

    public CreateNotificationMessageHandler(NotificationCreationSessionManager session)
    {
        _session = session;
    }

    public bool CanHandle(Message msg, UserState state)
        => _session.HasDraft(msg.From!.Id);

    public async Task HandleAsync(Message msg, ITelegramBotClient bot, CancellationToken token)
    {
        var draft = _session.GetOrCreateDraft(msg.From!.Id);
        
        // Логика поэтапного создания уведомления
        // Пример: если ещё нет title — ждём его и сохраняем в draft.Title
        // и т.д.

        await bot.SendMessage(msg.Chat.Id, "Продолжаем создание уведомления...", cancellationToken: token);
    }
}