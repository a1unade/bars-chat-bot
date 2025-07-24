using Microsoft.Extensions.Logging;
using NotifyHub.TelegramBot.Application.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace NotifyHub.TelegramBot.Infrastructure.Handlers;

public class UpdateHandler(
    IEnumerable<IMessageHandler> messageHandlers,
    IEnumerable<ICallbackHandler> callbackHandlers,
    IUserStateService userStateService,
    ILogger<UpdateHandler> logger)
    : IUpdateHandler
{
    public async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken ct)
    {
        switch (update.Type)
        {
            case UpdateType.Message:
                await HandleMessageAsync(bot, update.Message!, ct);
                break;

            case UpdateType.CallbackQuery:
                await HandleCallbackQueryAsync(bot, update.CallbackQuery!, ct);
                break;

            default:
                logger.LogInformation("Update type {UpdateType} not handled.", update.Type);
                break;
        }
    }

    public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Ошибка в Telegram-боте. Source: {Source}", source);
        await Task.CompletedTask;
    }

    private async Task HandleMessageAsync(ITelegramBotClient bot, Message message, CancellationToken ct)
    {
        var userId = message.From?.Id ?? 0;
        if (userId == 0)
            return;

        var state = userStateService.GetState(userId);

        foreach (var handler in messageHandlers)
        {
            if (handler.CanHandle(message, state))
            {
                await handler.HandleAsync(message, bot, ct);
                return;
            }
        }

        logger.LogWarning("No message handler found for user {UserId} with state {State}", userId, state);
    }

    private async Task HandleCallbackQueryAsync(ITelegramBotClient bot, CallbackQuery query, CancellationToken ct)
    {
        foreach (var handler in callbackHandlers)
        {
            if (handler.CanHandle(query.Data!))
            {
                await handler.HandleAsync(query, bot, ct);
                return;
            }
        }

        logger.LogWarning("No callback handler found for callback data: {CallbackData}", query.Data);
    }
}