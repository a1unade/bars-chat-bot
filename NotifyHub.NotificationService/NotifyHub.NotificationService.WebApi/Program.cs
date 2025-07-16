using NotifyHub.NotificationService.Infrastructure.Extensions;
using NotifyHub.NotificationService.Persistence.Extensions;
using NotifyHub.NotificationService.WebApi.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Регистрация сервисов
builder.Services.AddInfrastructureLayer();
// Регистрация контекста базы данных и репозиториев
builder.Services.AddPersistenceLayer(builder.Configuration);

var app = builder.Build();

// Применение миграций
await app.UseMigrations();

app.Run();
