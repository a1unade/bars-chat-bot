using NotifyHub.Kafka.Interfaces;

namespace NotifyHub.OutboxProcessor.WebApi.Configurations;

public static class KafkaConfiguration
{
    public static async Task UseKafka(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var initializer = scope.ServiceProvider.GetRequiredService<IKafkaTopicsInitializer>();
            await initializer.EnsureTopicsCreatedAsync();
        }
    }
}