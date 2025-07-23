namespace NotifyHub.TelegramBot.Application.Responses;

public class GraphQlResponse<T>
{
    public T Data { get; set; } = default!;
}