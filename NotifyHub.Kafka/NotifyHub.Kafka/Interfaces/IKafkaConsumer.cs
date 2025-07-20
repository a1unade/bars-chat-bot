namespace NotifyHub.Kafka.Interfaces;

/// <summary>
/// Kafka-consumer, обрабатывающий входящие сообщения.
/// </summary>
public interface IKafkaConsumer<TMessage> where TMessage : class
{
    /// <summary>
    /// Получение сообщений из Kafka.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    Task<TMessage?> ConsumeAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Закрывает Kafka-consumer и освобождает ресурсы.
    /// </summary>
    void Close();
}
