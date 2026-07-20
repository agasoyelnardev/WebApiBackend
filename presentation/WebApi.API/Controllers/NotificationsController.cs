using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Application.Features.Notifications.Commands.DeleteNotification;
using WebApi.Application.Features.Notifications.Commands.MarkAllAsRead;
using WebApi.Application.Features.Notifications.Commands.ToggleRead;
using WebApi.Application.Features.Notifications.Queries.GetNotifications;
using WebApi.Application.Features.Notifications.Queries.GetUnreadNotificationsCount;
using WebApi.Application.Interfaces;

namespace WebApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public NotificationsController(IMediator mediator, ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    [HttpGet]
    public async Task<IActionResult> GetNotifications(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] bool unreadOnly = false)
    {
        var result = await _mediator.Send(new GetNotificationsQuery
        {
            UserId = _currentUserService.UserId,
            PageNumber = page,
            PageSize = pageSize,
            UnreadOnly = unreadOnly
        });

        return Ok(result);
    }

    [HttpGet("unread-count")]
    public async Task<IActionResult> GetUnreadCount()
    {
        var count = await _mediator.Send(new GetUnreadNotificationsCountQuery
        {
            UserId = _currentUserService.UserId
        });

        return Ok(count);
    }

    [HttpPut("{id}/toggle-read")]
    public async Task<IActionResult> ToggleRead(Guid id)
    {
        await _mediator.Send(new ToggleNotificationReadCommand
        {
            Id = id,
            UserId = _currentUserService.UserId
        });

        return NoContent();
    }

    [HttpPut("mark-all-read")]
    public async Task<IActionResult> MarkAllAsRead()
    {
        await _mediator.Send(new MarkAllNotificationsAsReadCommand
        {
            UserId = _currentUserService.UserId
        });

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteNotificationCommand
        {
            Id = id,
            UserId = _currentUserService.UserId
        });

        return NoContent();
    }
}