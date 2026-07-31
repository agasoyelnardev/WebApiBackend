using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Application.Features.Admin.Queries.GetAdminStats;
using WebApi.Application.Features.Admin.Queries.GetRecentActivity;

namespace WebApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        var stats = await _mediator.Send(new GetAdminStatsQuery());
        return Ok(stats);
    }
    
    [HttpGet("recent-activity")]
    public async Task<IActionResult> GetRecentActivity()
    {
        var result = await _mediator.Send(new GetRecentActivityQuery());
        return Ok(result);
    }
}