using Microsoft.Extensions.Logging;
using NotifyHub.Infrastructure.Processors;

namespace NotifyHub.Infrastructure.Jobs;

public class OutboxJob(OutboxProcessor processor, ILogger<OutboxJob> logger)
{
    public async Task Execute(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Starting outbox job");
        await processor.ProcessAsync(cancellationToken);
    }
}