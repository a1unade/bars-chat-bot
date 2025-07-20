namespace NotifyHub.Kafka.Options;

/// <summary>
/// Настройки для Kafka
/// </summary>
public class KafkaOptions
{
    /// <summary>
    /// Топик для отправки сообщений
    /// </summary>
    public required string Topic { get; set; }
    
    /// <summary>
    /// Адреса Kafka-брокеров
    /// </summary>
    public required string BootstrapServers { get; set; }
}