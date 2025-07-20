namespace NotifyHub.Kafka.Options;

/// <summary>
/// Настройки для Kafka
/// </summary>
public class KafkaOptions
{
    /// <summary>
    /// Адреса Kafka-брокеров
    /// </summary>
    public required string BootstrapServers { get; set; }

    /// <summary>
    /// Топики для продюсеров
    /// </summary>
    public required Dictionary<string, string> ProducerTopics { get; set; }

    /// <summary>
    /// Топики для консюмеров
    /// </summary>
    public required Dictionary<string, string> ConsumerTopics { get; set; }
}