using Confluent.Kafka;
using NotifyHub.Kafka.Options;
using NotifyHub.Kafka.Interfaces;
using Microsoft.Extensions.Options;
using NotifyHub.Kafka.Configurations;

namespace NotifyHub.Kafka.Services;

public class KafkaProducer<TMessage> : IKafkaProducer<TMessage>
{
    private readonly IProducer<string, TMessage> _producer;
    
    private readonly string _topic;
    
    public KafkaProducer(IOptions<KafkaOptions> options)
    {
        ProducerConfig config = new ProducerConfig
        {
            BootstrapServers = options.Value.BootstrapServers,
        };

        _producer = new ProducerBuilder<string, TMessage>(config)
            .SetValueSerializer(new KafkaJsonSerializer<TMessage>())
            .Build();
        
        _topic = options.Value.Topic;
    }
    
    public async Task ProduceAsync(string key, TMessage message, CancellationToken cancellationToken = default)
    {
        await _producer.ProduceAsync(_topic, new Message<string, TMessage>
        {
            Key = key,
            Value = message
        }, cancellationToken);
    }
}