using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Application.Features.Users.Commands.DeleteUser;
using WebApi.Application.Features.Users.Commands.SetUserRole;
using WebApi.Application.Features.Users.Commands.SetUserStatus;
using WebApi.Application.Features.Users.Commands.ToggleUserRole;
using WebApi.Application.Features.Users.Commands.UpdateProfile;
using WebApi.Application.Features.Users.Dtos;
using WebApi.Application.Features.Users.Queries.GetAllUsers;
using WebApi.Application.Features.Users.Queries.GetUserProfile;

namespace WebApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var user = await _mediator.Send(new GetUserProfileQuery(id));

        if (user is null)
            return NotFound();

        return Ok(user);
    }

    [Authorize]
    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile(UpdateProfileCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }
    
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? searchTerm, [FromQuery] string? role, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var users = await _mediator.Send(new GetAllUsersQuery
        {
            SearchTerm = searchTerm,
            Role = role,
            PageNumber = page,
            PageSize = pageSize
        });

        return Ok(users);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{userId}/toggle-role")]
    public async Task<IActionResult> ToggleRole(string userId)
    {
        await _mediator.Send(new ToggleUserRoleCommand { UserId = userId });
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        await _mediator.Send(new DeleteUserCommand(userId));

        return NoContent();
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}/role")]
    public async Task<IActionResult> SetRole(string id, [FromBody] SetRoleRequest request)
    {
        var command = new SetUserRoleCommand { UserId = id, Role = request.Role };
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}/status")]
    public async Task<IActionResult> SetStatus(string id, [FromBody] SetStatusRequest request)
    {
        var command = new SetUserStatusCommand
        {
            UserId = id,
            IsBlocked = request.IsBlocked,
            Reason = request.Reason
        };
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}