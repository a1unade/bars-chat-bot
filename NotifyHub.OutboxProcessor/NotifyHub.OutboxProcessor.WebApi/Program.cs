using NotifyHub.OutboxProcessor.Infrastructure.Extensions;
using NotifyHub.OutboxProcessor.Persistence.Extensions;
using NotifyHub.OutboxProcessor.WebApi.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Регистрация outbox processor
builder.Services.AddInfrastructureLayer();
// Регистрация контекста базы данных и репозиториев
builder.Services.AddPersistenceLayer(builder.Configuration);

var app = builder.Build();

// Применение миграций
await app.UseMigrations();
// Hangfire jobs
app.UseHangfire();

app.Run();