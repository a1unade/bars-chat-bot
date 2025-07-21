using Confluent.Kafka;
using NotifyHub.Kafka.Options;
using NotifyHub.Kafka.Interfaces;
using Microsoft.Extensions.Options;
using NotifyHub.Kafka.Configurations;
using NotifyHub.Kafka.Exceptions;

namespace NotifyHub.Kafka.Services;

/// <summary>
/// Kafka-consumer для обработки сообщений определённого типа.
/// </summary>
/// <typeparam name="TMessage">Тип ожидаемого сообщения.</typeparam>
public class KafkaConsumer<TMessage> : IKafkaConsumer<TMessage> where TMessage : class
{
    private readonly IConsumer<string, TMessage> _consumer;
    private Exception? _kafkaError;
    private readonly Dictionary<string, string> _consumerTopics;

    public KafkaConsumer(IOptions<KafkaOptions> options)
    {
        _consumerTopics = options.Value.ConsumerTopics;

        var config = new ConsumerConfig
        {
            BootstrapServers = options.Value.BootstrapServers,
            GroupId = $"group-{typeof(TMessage).Name.ToLower()}",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = true
        };

        _consumer = new ConsumerBuilder<string, TMessage>(config)
            .SetValueDeserializer(new KafkaJsonDeserializer<TMessage>())
            .SetErrorHandler((_, e) =>
            {
                var ex = new KafkaConsumeException("Kafka consume error: { 0 }", e.Reason);
                _kafkaError = ex;
            })
            .Build();

        _consumer.Subscribe(_consumerTopics.Values);
    }

    public Task<TMessage?> ConsumeAsync(string topicKey, CancellationToken cancellationToken)
    {
        if (_kafkaError is not null)
            throw _kafkaError;
        
        if (!_consumerTopics.TryGetValue(topicKey, out var topicName))
            throw new ArgumentException($"Topic key '{topicKey}' not found in ConsumerTopics.", nameof(topicKey));

        try
        {
            _consumer.Subscribe(topicName);

            var result = _consumer.Consume(cancellationToken);
            return Task.FromResult(result?.Message?.Value);
        }
        catch (ConsumeException ex)
        {
            throw new KafkaConsumeException($"Consume error from topic '{topicName}': {ex.Error.Reason}", ex);
        }
    }

    public void Close() => _consumer.Close();
}