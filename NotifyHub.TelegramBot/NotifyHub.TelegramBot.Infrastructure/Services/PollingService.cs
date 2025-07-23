using Microsoft.Extensions.Logging;
using NotifyHub.TelegramBot.Infrastructure.Services.Common;

namespace NotifyHub.TelegramBot.Infrastructure.Services;

public class PollingService(IServiceProvider serviceProvider, ILogger<PollingService> logger)
    : PollingServiceBase<ReceiverService>(serviceProvider, logger);