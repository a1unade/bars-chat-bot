using System.Text.Json;

namespace NotifyHub.NotificationService.WebApi.Middlewares;

/// <summary>
/// Middleware для валидации пустых полей в запросе
/// </summary>
public class RequestValidationMiddleware
{
    private readonly RequestDelegate _next;

    public RequestValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        context.Request.EnableBuffering();

        if (context.Request.ContentLength > 0 &&
            context.Request.ContentType != null &&
            context.Request.ContentType.Contains("application/json"))
        {
            using var reader = new StreamReader(
                context.Request.Body,
                encoding: System.Text.Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                bufferSize: 1024,
                leaveOpen: true
            );

            var body = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;

            var dict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(body);

            if (dict is not null)
            {
                var emptyFields = dict
                    .Where(x => IsValueEmpty(x.Value))
                    .Select(x => x.Value)
                    .ToList();

                if (emptyFields.Any())
                {
                    var error = new
                    {
                        Message = "Validation failed. Empty fields are not allowed.",
                        EmptyFields = emptyFields
                    };

                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonSerializer.Serialize(error));
                    return;
                }
            }
        }

        await _next(context);
    }
    
    /// <summary>
    /// Проверка на пустые поля в запросе
    /// </summary>
    private static bool IsValueEmpty(JsonElement value)
    {
        return value.ValueKind switch
        {
            JsonValueKind.String => string.IsNullOrWhiteSpace(value.GetString()),

            JsonValueKind.Null => true,

            JsonValueKind.Object => !value.EnumerateObject().Any(),

            JsonValueKind.Array => value.GetArrayLength() == 0,

            JsonValueKind.Undefined => true,

            _ => false
        };
    }
}