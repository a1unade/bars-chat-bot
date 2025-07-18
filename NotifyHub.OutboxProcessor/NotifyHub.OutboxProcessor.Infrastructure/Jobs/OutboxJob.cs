using Microsoft.Extensions.Logging;

namespace NotifyHub.OutboxProcessor.Infrastructure.Jobs;

public class OutboxJob(Processors.OutboxProcessor processor, ILogger<OutboxJob> logger)
{
    public async Task Execute(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Starting outbox job");
        await processor.ProcessAsync(cancellationToken);
    }
}