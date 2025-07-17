namespace NotifyHub.NotificationService.Application.Common.Requests;

public class IdRequest
{
    /// <summary>
    /// ID для запроса сущности
    /// </summary>
    public Guid Id { get; set; } 
    
    public IdRequest()
    {
    }

    public IdRequest(IdRequest requests)
    {
        Id = requests.Id;
    }
}