using Confluent.Kafka;
using System.Text.Json;

namespace NotifyHub.Kafka.Configurations;

public class KafkaJsonDeserializer<TMessage>: IDeserializer<TMessage>
{
    public TMessage Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        return JsonSerializer.Deserialize<TMessage>(data, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        })!;
    }
}