using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace NotifyHub.WebApi.Configurations;

public static class SerilogConfiguration
{
    public static void AddSerilog(this IServiceCollection services, IConfiguration configuration)
    {
        Log.Logger = ConfigureSerilog(configuration).CreateLogger();

        services.AddLogging(log =>
        {
            log.ClearProviders();
            log.AddSerilog(dispose: true);
        });
    }

    private static LoggerConfiguration ConfigureSerilog(IConfiguration configuration)
    {
        return new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", "WebApi")
            .WriteTo.Console(
                theme: AnsiConsoleTheme.Literate,
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
            )
            .WriteTo.File(
                path: "Logs/webapp.log",
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7,
                outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Level:u3}] {Message:lj}{NewLine}{Exception}"
            )
            .WriteTo.Sentry(o =>
            {
                o.Dsn = configuration["Sentry:Dsn"];
                o.Debug = true;
                o.MinimumEventLevel = LogEventLevel.Information;
            });
    }
}