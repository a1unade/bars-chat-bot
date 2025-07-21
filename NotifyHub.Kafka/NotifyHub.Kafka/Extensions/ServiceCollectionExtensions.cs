using Microsoft.Extensions.Configuration;
using NotifyHub.Kafka.Options;
using NotifyHub.Kafka.Services;
using NotifyHub.Kafka.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace NotifyHub.Kafka.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Регистрация Kafka-producer с поддержкой сериализации сообщений.
    /// </summary>
    public static IServiceCollection AddKafkaProducer<TMessage>(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<KafkaOptions>(configuration.GetSection("Kafka"));
        services.AddSingleton<IKafkaProducer<TMessage>, KafkaProducer<TMessage>>();

        return services;
    }
    
    /// <summary>
    /// Регистрирует Kafka-consumer с поддержкой десериализации сообщений.
    /// </summary>
    public static IServiceCollection AddKafkaConsumer<TMessage>(
        this IServiceCollection services, IConfiguration configuration)
        where TMessage : class
    {
        services.Configure<KafkaOptions>(configuration.GetSection("Kafka"));
        services.AddSingleton<IKafkaConsumer<TMessage>, KafkaConsumer<TMessage>>();
        
        return services;
    }

    /// <summary>
    /// Регистрация инциализатора топиков
    /// </summary>
    public static IServiceCollection AddKafkaTopicsInitializer(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<KafkaOptions>(configuration.GetSection("Kafka"));
        services.AddTransient<IKafkaTopicsInitializer, KafkaTopicInitializer>();
        
        return services;
    }
}