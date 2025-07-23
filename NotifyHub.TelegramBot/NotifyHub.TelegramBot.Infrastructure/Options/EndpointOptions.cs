namespace NotifyHub.TelegramBot.Infrastructure.Options;

public class EndpointOptions
{
    public string Graphql { get; set; } = default!;
    public string NotificationService { get; set; } = default!;
}