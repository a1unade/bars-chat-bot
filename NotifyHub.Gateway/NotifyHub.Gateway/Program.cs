var builder = WebApplication.CreateBuilder(args);

// Конфигурация для прокси
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.UseRouting();

app.MapReverseProxy();

app.Run();