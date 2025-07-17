using MediatR;
using Microsoft.AspNetCore.Mvc;
using NotifyHub.NotificationService.Application.Common.Requests;
using NotifyHub.NotificationService.Application.Common.Requests.History;
using NotifyHub.NotificationService.Application.Features.Queries.GetAllHistory;
using NotifyHub.NotificationService.Application.Features.Queries.GetHistoryById;

namespace NotifyHub.NotificationService.WebApi.Controllers;

/// <summary>
/// Контроллер для запросов записей об истории отправки
/// </summary>
[ApiController]
[Route("[controller]")]
public class HistoryController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    
    /// <summary>
    /// Получить все записи об отправке
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Записи об отправке</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetHistoryResponse))]
    public async Task<GetHistoryResponse> GetProjectsAsync(CancellationToken cancellationToken)
        => await _mediator.Send(new GetAllHistoryQuery(), cancellationToken);
    
    /// <summary>
    /// Получить запись об отправке по Id
    /// </summary>
    /// <param name="historyid">Id записи об отправке</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Запись об отправке</returns>
    [HttpGet("{historyid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetHistoryByIdResponse))]
    public async Task<GetHistoryByIdResponse> GetProjectByIdAsync([FromRoute] Guid historyid, CancellationToken cancellationToken)
        => await _mediator.Send(new GetHistoryByIdQuery( new IdRequest{ Id = historyid }), cancellationToken);
}