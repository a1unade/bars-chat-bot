using NotifyHub.OutboxProcessor.Application.Extensions;
using NotifyHub.OutboxProcessor.Infrastructure.Extensions;
using NotifyHub.OutboxProcessor.Persistence.Extensions;
using NotifyHub.OutboxProcessor.WebApi.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Регистрация AutoMapper
builder.Services.AddApplicationLayer();
// Регистрация outbox processor и Kafka
builder.Services.AddInfrastructureLayer(builder.Configuration);
// Регистрация контекста базы данных и репозиториев
builder.Services.AddPersistenceLayer(builder.Configuration);
// Настройка serilog + sentry
builder.Services.AddSerilog(builder.Configuration);

var app = builder.Build();

// Применение миграций
await app.UseMigrations();
// Создание топиков в Kafka
await app.UseKafka();
// Hangfire jobs
app.UseHangfire();

app.Run();