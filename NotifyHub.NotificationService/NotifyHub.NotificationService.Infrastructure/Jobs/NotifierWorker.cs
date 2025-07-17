using Microsoft.Extensions.Hosting;

namespace NotifyHub.NotificationService.Infrastructure.Jobs;

public class NotifierWorker: BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // TODO: реализовать воркер для отправки уведомлений, пакет quartz/hangfire
        
        // TODO: добавить логирование
        await Task.CompletedTask;
    }
}