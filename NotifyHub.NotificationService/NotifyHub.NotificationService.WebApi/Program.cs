using System.Reflection;
using NotifyHub.NotificationService.Application.Extensions;
using NotifyHub.NotificationService.Infrastructure.Extensions;
using NotifyHub.NotificationService.Persistence.Extensions;
using NotifyHub.NotificationService.WebApi.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Регистрация MediatR
builder.Services.AddApplicationLayer();
// Регистрация сервисов
builder.Services.AddInfrastructureLayer();
// Регистрация контекста базы данных и репозиториев
builder.Services.AddPersistenceLayer(builder.Configuration);

builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

// Применение миграций
await app.UseMigrations();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Notification API");
    c.RoutePrefix = "swagger";
});

// Cors политика для gateway
app.UseCorsPolicy();
// Обработка исключений
app.UseExceptionMiddleware();
// Обработка пустых запросов
app.UseValidationMiddleware();

app.MapControllers();

app.Run();
