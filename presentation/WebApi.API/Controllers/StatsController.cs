using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Application.Features.Stats.Queries.GetPublicStats;
using WebApi.Application.Interfaces;

namespace WebApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatsController : ControllerBase
{
    private readonly IOnlineUsersTracker _onlineUsersTracker;
    private readonly IMediator _mediator;

    public StatsController(IOnlineUsersTracker onlineUsersTracker, IMediator mediator)
    {
        _onlineUsersTracker = onlineUsersTracker;
        _mediator = mediator;
    }

    [HttpGet("online-count")]
    public IActionResult GetOnlineCount()
    {
        return Ok(new { OnlineCount = _onlineUsersTracker.GetOnlineCount() });
    }
    
    [HttpGet]
    public async Task<IActionResult> GetPublicStats()
    {
        var stats = await _mediator.Send(new GetPublicStatsQuery());
        return Ok(stats);
    }
}