namespace NotifyHub.WebApi.Jobs;

public class OutboxNotifier: BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // TODO: реализовать outbox pattern для отправки уведомлений
        throw new NotImplementedException();
    }
}