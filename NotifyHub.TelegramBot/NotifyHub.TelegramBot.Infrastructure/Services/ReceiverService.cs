using Microsoft.Extensions.Logging;
using NotifyHub.TelegramBot.Infrastructure.Services.Common;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace NotifyHub.TelegramBot.Infrastructure.Services;

public class ReceiverService(ITelegramBotClient botClient, IUpdateHandler updateHandler, ILogger<ReceiverServiceBase<IUpdateHandler>> logger)
    : ReceiverServiceBase<IUpdateHandler>(botClient, updateHandler, logger);