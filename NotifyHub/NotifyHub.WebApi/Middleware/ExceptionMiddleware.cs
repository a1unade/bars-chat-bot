using System.Net;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;
using NotifyHub.Application.Common.Exceptions;

namespace NotifyHub.WebApi.Middleware;

public class ExceptionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            await HandleExceptionAsync(context, e);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError;
        var result = String.Empty;

        switch (exception)
        {
            case BaseException baseException:
                if (baseException.StatusCode != default)
                    code = baseException.StatusCode;
                result = JsonSerializer.Serialize(new { Error = baseException.Message });
                break;
            
            case ValidationException validationException:
                code = HttpStatusCode.BadRequest;
                result = JsonSerializer.Serialize(new {Error = validationException.Message});
                break;
            
            case NotFoundException notFoundException:
                code = HttpStatusCode.NotFound;
                result = JsonSerializer.Serialize(new { Error = notFoundException.Message });
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        if (result == String.Empty)
        {
            result = JsonSerializer.Serialize(new { Error = exception.Message });
        }

        return context.Response.WriteAsync(result);
    }
}