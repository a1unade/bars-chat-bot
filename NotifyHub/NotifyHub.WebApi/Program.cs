using NotifyHub.Application.Extensions;
using NotifyHub.Persistence.Extensions;
using NotifyHub.Infrastructure.Extensions;
using NotifyHub.WebApi.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Регистрация query и mutations
builder.Services.AddApplicationLayer();
// Регистрация graphql и kafka
builder.Services.AddInfrastructureLayer(builder.Configuration);
// Регистрация контекста базы данных и репозиториев
builder.Services.AddPersistenceLayer(builder.Configuration);

builder.Services.AddCors();
// Настройка serilog + sentry
builder.Services.AddSerilog(builder.Configuration);

var app = builder.Build();

// Применение миграций
await app.UseMigrations();
// Создание топиков в Kafka
await app.UseKafka();

// Применение CORS политики для gateway
app.UseCorsPolicy();
// GraphQL 
app.MapGraphQL();

app.Run();