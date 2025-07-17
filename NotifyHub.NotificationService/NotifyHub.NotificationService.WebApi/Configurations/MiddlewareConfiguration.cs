using NotifyHub.NotificationService.WebApi.Middlewares;

namespace NotifyHub.NotificationService.WebApi.Configurations;

public static class MiddlewareConfiguration
{
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionMiddleware>();
    }

    public static IApplicationBuilder UseValidationMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestValidationMiddleware>();
    }
}