using NotifyHub.WebApi.Middleware;

namespace NotifyHub.WebApi.Configurations;

public static class MiddlewareConfiguration
{
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionMiddleware>();
    }
}