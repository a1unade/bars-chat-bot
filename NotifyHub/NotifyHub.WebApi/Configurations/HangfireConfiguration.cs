using Hangfire;
using NotifyHub.Infrastructure.Jobs;

namespace NotifyHub.WebApi.Configurations;

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