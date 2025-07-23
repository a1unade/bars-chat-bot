using Microsoft.Extensions.Logging;
using NotifyHub.TelegramBot.Infrastructure.Handlers;
using NotifyHub.TelegramBot.Infrastructure.Services.Common;
using Telegram.Bot;

namespace NotifyHub.TelegramBot.Infrastructure.Services;

public class ReceiverService(ITelegramBotClient botClient, UpdateHandler updateHandler, ILogger<ReceiverServiceBase<UpdateHandler>> logger)
    : ReceiverServiceBase<UpdateHandler>(botClient, updateHandler, logger);