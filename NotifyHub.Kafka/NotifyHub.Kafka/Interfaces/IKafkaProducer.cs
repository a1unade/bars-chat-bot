namespace NotifyHub.Kafka.Interfaces;

/// <summary>
/// Kafka-producer сообщений
/// </summary>
/// <typeparam name="TMessage">Тип отправляемого сообщения.</typeparam>
public interface IKafkaProducer<in TMessage>
{
    /// <summary>
    /// Отправляет сообщение в Kafka.
    /// </summary>
    /// <param name="key">Ключ маршрутизации</param>
    /// <param name="message">Сообщение для отправки.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    Task ProduceAsync(string key, TMessage message, CancellationToken cancellationToken);
}
