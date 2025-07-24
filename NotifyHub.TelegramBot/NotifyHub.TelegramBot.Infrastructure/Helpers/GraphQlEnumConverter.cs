using System.Text.Json;
using System.Text.Json.Serialization;

namespace NotifyHub.TelegramBot.Infrastructure.Helpers;

public class GraphQlEnumConverter : JsonConverter<Enum>
{
    public override Enum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => throw new NotImplementedException();

    public override void Write(Utf8JsonWriter writer, Enum value, JsonSerializerOptions options)
    {
        writer.WriteRawValue(value.ToString().ToUpperInvariant());
    }
}