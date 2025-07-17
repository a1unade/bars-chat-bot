using MediatR;
using NotifyHub.NotificationService.Application.Common.Requests;
using NotifyHub.NotificationService.Application.Common.Requests.History;

namespace NotifyHub.NotificationService.Application.Features.Queries.GetHistoryById;

public class GetHistoryByIdQuery: IdRequest, IRequest<GetHistoryByIdResponse>
{
    public GetHistoryByIdQuery(IdRequest request) : base(request)
    {
    }
}