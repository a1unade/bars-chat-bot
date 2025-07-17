using MediatR;
using NotifyHub.NotificationService.Application.Common.Requests.History;

namespace NotifyHub.NotificationService.Application.Features.Queries.GetAllHistory;

/// <summary>
/// Запрос на получение истории уведомдений
/// </summary>
public class GetAllHistoryQuery: IRequest<GetHistoryResponse> { }