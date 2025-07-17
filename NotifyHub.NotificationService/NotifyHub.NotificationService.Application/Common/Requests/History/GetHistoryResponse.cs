namespace NotifyHub.NotificationService.Application.Common.Requests.History;

/// <summary>
/// Ответ на запрос о получении истории отправок
/// </summary>
public class GetHistoryResponse
{
    /// <summary>
    /// Записи об отправке
    /// </summary>
    public required ICollection<GetHistoryResponseItem> History { get; set; } = [];
}