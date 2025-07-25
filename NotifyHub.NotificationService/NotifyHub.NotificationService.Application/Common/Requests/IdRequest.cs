namespace NotifyHub.NotificationService.Application.Common.Requests;

public class IdRequest
{
    /// <summary>
    /// IDп пользователя для запроса истории
    /// </summary>
    public long Id { get; set; } 
    
    public IdRequest()
    {
    }

    public IdRequest(IdRequest requests)
    {
        Id = requests.Id;
    }
}