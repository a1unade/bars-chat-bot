using Confluent.Kafka;
using System.Text.Json;

namespace NotifyHub.Kafka.Configurations;

/// <summary>
/// JSON-сериализатор сообщений Kafka
/// </summary>
public class KafkaJsonSerializer<TMessage> : ISerializer<TMessage>
{
    public byte[] Serialize(TMessage data, SerializationContext context)
    {
        if (data == null)
            return [];

        return JsonSerializer.SerializeToUtf8Bytes(data, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        });
    }
}