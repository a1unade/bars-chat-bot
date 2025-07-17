using System.Net;
using System.Text.Json;
using NotifyHub.NotificationService.Application.Common.Exceptions;
using NotifyHub.NotificationService.Application.Common.Responses;

namespace NotifyHub.NotificationService.WebApi.Middlewares;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex) 
        {
            await HandleExceptionAsync(context, ex);
        }
        catch (NotFoundException ex) 
        {
            await HandleExceptionAsync(context, ex);
        }
        catch (Exception ex) 
        {
            logger.LogError(ex, "Unhandled exception occurred during request processing.");

            await HandleExceptionAsync(context, ex);
        }

    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError;
        var response = new ErrorResponse
        {
            Code = "INTERNAL_ERROR",
            Message = "An unexpected error occurred."
        };

        switch (exception)
        {
            case ValidationException validationException:
                code = HttpStatusCode.BadRequest;
                response.Code = "VALIDATION_ERROR";
                response.Message = "Validation failed.";
                response.Errors = validationException.Errors;
                break;
            
            case NotFoundException notFoundException:
                code = HttpStatusCode.NotFound;
                response.Code = "NOT_FOUND";
                response.Message = notFoundException.Message;
                break;

            case BaseException baseException:
                code = baseException.StatusCode != default ? baseException.StatusCode : HttpStatusCode.InternalServerError;
                response.Code = baseException.GetType().Name.ToUpperInvariant();
                response.Message = baseException.Message;
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}