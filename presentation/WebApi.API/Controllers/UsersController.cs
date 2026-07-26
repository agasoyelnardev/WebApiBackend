using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.Application.Features.Users.Commands.DeleteUser;
using WebApi.Application.Features.Users.Commands.ToggleUserRole;
using WebApi.Application.Features.Users.Commands.UpdateProfile;
using WebApi.Application.Features.Users.Queries.GetAllUsers;
using WebApi.Application.Features.Users.Queries.GetUserProfile;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;
    private readonly UserManager<AppUser> _userManager;

    public UsersController(IMediator mediator, ICurrentUserService currentUserService, UserManager<AppUser> userManager)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
        _userManager = userManager;
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
        command.UserId = _currentUserService.UserId;
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
        await _mediator.Send(new DeleteUserCommand
        {
            UserId = userId,
            RequestedByUserId = _currentUserService.UserId
        });

        return NoContent();
    }
}