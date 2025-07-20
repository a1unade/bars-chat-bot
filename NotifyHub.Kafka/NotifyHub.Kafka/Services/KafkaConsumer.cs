using Confluent.Kafka;
using NotifyHub.Kafka.Options;
using NotifyHub.Kafka.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NotifyHub.Kafka.Configurations;

namespace NotifyHub.Kafka.Services;

/// <summary>
/// Kafka-consumer для обработки сообщений определённого типа.
/// </summary>
/// <typeparam name="TMessage">Тип ожидаемого сообщения.</typeparam>
public class KafkaConsumer<TMessage> : IKafkaConsumer<TMessage> where TMessage : class
{
    private readonly ILogger<KafkaConsumer<TMessage>> _logger;
    private readonly IConsumer<string, TMessage> _consumer;

    public KafkaConsumer(
        IOptions<KafkaOptions> options,
        ILogger<KafkaConsumer<TMessage>> logger)
    {
        _logger = logger;

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
                _logger.LogError("Kafka consume error: {0}", e.Reason);
            })
            .Build();

        var topic = options.Value.Topic;
        _consumer.Subscribe(topic);
    }

    public Task<TMessage?> ConsumeAsync(CancellationToken cancellationToken)
    {
        try
        {
            var result = _consumer.Consume(cancellationToken);
            return Task.FromResult(result?.Message?.Value);
        }
        catch (ConsumeException ex)
        {
            _logger.LogError("Consume error: {0}", ex.Error.Reason);
            return Task.FromResult<TMessage?>(null);
        }
    }

    public void Close() => _consumer.Close();
}