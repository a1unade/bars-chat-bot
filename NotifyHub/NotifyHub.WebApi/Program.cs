using NotifyHub.Application.Extensions;
using NotifyHub.Persistence.Extensions;
using NotifyHub.Infrastructure.Extensions;
using NotifyHub.WebApi.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Регистрация query и mutations
builder.Services.AddApplicationLayer();
// Регистрация сервисов
builder.Services.AddInfrastructureLayer();
// Регистрация контекста базы данных и репозиториев
builder.Services.AddPersistenceLayer(builder.Configuration);

builder.Services.AddCors();

var app = builder.Build();

// Применение миграций
await app.UseMigrations();

// Применение CORS политики для gateway
app.UseCorsPolicy();
// Обработка исключений
app.UseExceptionMiddleware();
// GraphQL 
app.MapGraphQL();

app.Run();