using Hangfire;
using NotifyHub.OutboxProcessor.Infrastructure.Jobs;

namespace NotifyHub.OutboxProcessor.WebApi.Configurations;

public static class HangfireConfiguration
{
    public static IApplicationBuilder UseHangfire(this IApplicationBuilder builder)
    {
        builder.UseHangfireDashboard();

        RecurringJob.AddOrUpdate<OutboxJob>(
            recurringJobId: "outbox-job",
            methodCall: job => job.Execute(CancellationToken.None),
            cronExpression: Cron.Minutely,
            options: new RecurringJobOptions
            {
                TimeZone = TimeZoneInfo.Utc,
                MisfireHandling = MisfireHandlingMode.Relaxed
            });
        
        return builder;
    }
}