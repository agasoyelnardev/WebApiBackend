using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.Application.Features.Users.Commands.UpdateProfile;
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
}