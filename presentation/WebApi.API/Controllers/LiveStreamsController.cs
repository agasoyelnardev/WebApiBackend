using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Application.Features.LiveStreams.Commands.CreateLiveStream;
using WebApi.Application.Features.LiveStreams.Commands.ToggleLiveStream;
using WebApi.Application.Features.LiveStreams.Queries.GetActiveLiveStreams;
using WebApi.Application.Features.LiveStreams.Queries.GetLiveStreamById;
using WebApi.Application.Features.LiveStreams.Queries.GetLiveStreamChatHistory;
using WebApi.Application.Features.LiveStreams.Queries.GetLiveStreamSchedule;

namespace WebApi.API.Controllers;

[ApiController]
[Route("api/livestreams")]
public class LiveStreamsController : ControllerBase
{
    private readonly IMediator _mediator;

    public LiveStreamsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var streams = await _mediator.Send(new GetActiveLiveStreamsQuery(), cancellationToken);
        return Ok(streams);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var stream = await _mediator.Send(new GetLiveStreamByIdQuery(id), cancellationToken);

        if (stream is null)
            return NotFound();

        return Ok(stream);
    }

    [HttpGet("{id}/chat-history")]
    public async Task<IActionResult> GetChatHistory(Guid id, CancellationToken cancellationToken)
    {
        var messages = await _mediator.Send(new GetLiveStreamChatHistoryQuery(id), cancellationToken);
        return Ok(messages);
    }

    [HttpGet("schedule")]
    public async Task<IActionResult> GetSchedule(CancellationToken cancellationToken)
    {
        var schedule = await _mediator.Send(new GetLiveStreamScheduleQuery(), cancellationToken);
        return Ok(schedule);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("admin/toggle-live")]
    public async Task<IActionResult> ToggleLive([FromBody] ToggleLiveStreamCommand command, CancellationToken cancellationToken)
    {
        var isLive = await _mediator.Send(command, cancellationToken);
        return Ok(new { IsLive = isLive });
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPost("admin")]
    public async Task<IActionResult> Create([FromBody] CreateLiveStreamCommand command, CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(command, cancellationToken);
        return Ok(new { Id = id });
    }
}