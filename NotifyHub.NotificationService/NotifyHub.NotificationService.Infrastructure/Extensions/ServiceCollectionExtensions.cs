using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotifyHub.Abstractions.DTOs;
using NotifyHub.Kafka.Extensions;
using NotifyHub.NotificationService.Application.Interfaces;
using NotifyHub.NotificationService.Infrastructure.Behaviours;
using NotifyHub.NotificationService.Infrastructure.Services;
using NotifyHub.NotificationService.Infrastructure.Workers;

namespace NotifyHub.NotificationService.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddServices();
        services.AddKafka(configuration);
        services.AddWorkers();
        services.AddPipelineBehaviours();
    }
    
    private static void AddServices(this IServiceCollection services) => 
        services.AddScoped<IEmailService, EmailService>();

    private static void AddKafka(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddKafkaConsumer<NotificationMessageDto>(configuration)
            .AddKafkaProducer<NotificationMessageDto>(configuration)
            .AddKafkaTopicsInitializer(configuration);
    }
    
    private static void AddWorkers(this IServiceCollection services) =>
        services.AddHostedService<KafkaBackgroundService>();
    
    private static void AddPipelineBehaviours(this IServiceCollection services) => 
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
}