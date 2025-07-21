using Confluent.Kafka;
using NotifyHub.Kafka.Options;
using NotifyHub.Kafka.Interfaces;
using Microsoft.Extensions.Options;
using NotifyHub.Kafka.Configurations;
using NotifyHub.Kafka.Exceptions;

namespace NotifyHub.Kafka.Services;

public class KafkaProducer<TMessage> : IKafkaProducer<TMessage>
{
    private readonly IProducer<string, TMessage> _producer;
    private readonly KafkaOptions _options;
    
    public KafkaProducer(IOptions<KafkaOptions> options)
    {
        _options = options.Value;
        
        ProducerConfig config = new ProducerConfig
        {
            BootstrapServers = options.Value.BootstrapServers,
        };

        _producer = new ProducerBuilder<string, TMessage>(config)
            .SetValueSerializer(new KafkaJsonSerializer<TMessage>())
            .Build();
    }
    
    public async Task ProduceAsync(string topicKey, string key, TMessage message, CancellationToken cancellationToken = default)
    {
        if (!_options.ProducerTopics.TryGetValue(topicKey, out var topicName))
            throw new KafkaProduceException("Unknown topic key: {0}", topicKey);
        
        await _producer.ProduceAsync(topicName, new Message<string, TMessage>
        {
            Key = key,
            Value = message
        }, cancellationToken);
    }
}