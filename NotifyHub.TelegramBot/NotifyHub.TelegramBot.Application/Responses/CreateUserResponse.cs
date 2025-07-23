using System.Text.Json.Serialization;

namespace NotifyHub.TelegramBot.Application.Responses;

public class CreateUserResponse
{
    [JsonPropertyName("createUser")]
    public Guid CreateUser { get; set; }
}