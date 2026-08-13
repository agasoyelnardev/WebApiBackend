using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Application.Common.Models;
using WebApi.Application.Features.Admin.Commands.AddUserPoints;
using WebApi.Application.Features.Admin.Commands.CloseStreamRoom;
using WebApi.Application.Features.Admin.Commands.DeleteBookReview;
using WebApi.Application.Features.Admin.Commands.DeleteReview;
using WebApi.Application.Features.Admin.Commands.DeleteUser;
using WebApi.Application.Features.Admin.Commands.ToggleUserBan;
using WebApi.Application.Features.Admin.Commands.UpdateUserRoles;
using WebApi.Application.Features.Admin.Dtos;
using WebApi.Application.Features.Admin.Queries.GetAdminActivityLogs;
using WebApi.Application.Features.Admin.Queries.GetAdminStats;
using WebApi.Application.Features.Admin.Queries.GetAdminUsers;
using WebApi.Application.Features.Admin.Queries.GetRecentActivity;

namespace WebApi.API.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("stats")]
    public async Task<ActionResult<AdminStatsDto>> GetStats(CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new GetAdminStatsQuery(), cancellationToken));
    }

    [HttpGet("users")]
    public async Task<ActionResult<PaginatedList<AdminUserDto>>> GetUsers(
        [FromQuery] string? search,
        [FromQuery] string? role,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var query = new GetAdminUsersQuery
        {
            Search = search,
            Role = role,
            Page = page,
            PageSize = pageSize
        };

        return Ok(await _mediator.Send(query, cancellationToken));
    }

    [HttpPost("users/{userId}/toggle-ban")]
    public async Task<ActionResult> ToggleBan(string userId, [FromBody] string? banReason, CancellationToken cancellationToken)
    {
        var isBanned = await _mediator.Send(new ToggleUserBanCommand { UserId = userId, BanReason = banReason }, cancellationToken);
        return Ok(new { isBanned });
    }

    [HttpPost("users/{userId}/roles")]
    public async Task<ActionResult> UpdateRoles(string userId, [FromBody] List<string> roles, CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdateUserRolesCommand { UserId = userId, Roles = roles }, cancellationToken);
        return NoContent();
    }

    [HttpPost("users/{userId}/add-points")]
    public async Task<ActionResult> AddPoints(string userId, [FromBody] int points, CancellationToken cancellationToken)
    {
        var newBalance = await _mediator.Send(new AddUserPointsCommand { UserId = userId, PointsToAdd = points }, cancellationToken);
        return Ok(new { points = newBalance });
    }

    [HttpDelete("users/{userId}")]
    public async Task<ActionResult> DeleteUser(string userId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteUserCommand { UserId = userId }, cancellationToken);
        return NoContent();
    }

    [HttpGet("activity-logs")]
    public async Task<ActionResult<List<AdminActivityLogDto>>> GetActivityLogs(CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new GetAdminActivityLogsQuery(), cancellationToken));
    }

    [HttpDelete("reviews/{id}")]
    public async Task<ActionResult> DeleteReview(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteReviewCommand { ReviewId = id }, cancellationToken);
        return NoContent();
    }

    [HttpDelete("book-reviews/{id}")]
    public async Task<ActionResult> DeleteBookReview(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteBookReviewCommand { BookReviewId = id }, cancellationToken);
        return NoContent();
    }

    [HttpPost("rooms/{id}/close")]
    public async Task<ActionResult> CloseRoom(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new CloseStreamRoomCommand { RoomId = id }, cancellationToken);
        return NoContent();
    }
    
    [HttpGet("recent-activity")]
    public async Task<ActionResult<RecentActivityDto>> GetRecentActivity(
        [FromQuery] int userCount = 10,
        [FromQuery] int reviewCount = 10,
        CancellationToken cancellationToken = default)
    {
        var query = new GetRecentActivityQuery
        {
            UserCount = userCount,
            ReviewCount = reviewCount
        };

        return Ok(await _mediator.Send(query, cancellationToken));
    }
}