namespace NotifyHub.NotificationService.Application.Common.Responses;

public class ErrorResponse
{
    public string Code { get; set; } = "INTERNAL_ERROR";
    
    public string Message { get; set; } = "Something went wrong.";
    
    public IDictionary<string, string[]>? Errors { get; set; } = null;
}