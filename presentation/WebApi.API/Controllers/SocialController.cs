using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Application.Features.Social.Commands.FollowUser;
using WebApi.Application.Features.Social.Commands.UnfollowUser;
using WebApi.Application.Features.Social.Queries.GetFollowers;
using WebApi.Application.Features.Social.Queries.GetFollowing;
using WebApi.Application.Interfaces;

namespace WebApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SocialController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public SocialController(
        IMediator mediator,
        ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    [HttpPost("follow/{userId}")]
    public async Task<IActionResult> Follow(string userId)
    {
        if (_currentUserService.UserId is null)
            return Unauthorized();

        var result = await _mediator.Send(
            new FollowUserCommand(userId)
            {
                FollowerUserId = _currentUserService.UserId
            });

        if (!result)
            return BadRequest("İzləmə əməliyyatı uğursuz oldu");

        return Ok("İstifadəçi izlənildi");
    }

    [HttpDelete("follow/{userId}")]
    public async Task<IActionResult> Unfollow(string userId)
    {
        if (_currentUserService.UserId is null)
            return Unauthorized();

        var result = await _mediator.Send(
            new UnfollowUserCommand(userId)
            {
                FollowerUserId = _currentUserService.UserId
            });

        if (!result)
            return BadRequest("İzləmədən çıxma əməliyyatı uğursuz oldu");

        return Ok("İzləmədədən çıxarıldı");
    }

    [AllowAnonymous]
    [HttpGet("followers/{userId}")]
    public async Task<IActionResult> GetFollowers(string userId)
    {
        var followers = await _mediator.Send(
            new GetFollowersQuery(userId));

        return Ok(followers);
    }

    [AllowAnonymous]
    [HttpGet("following/{userId}")]
    public async Task<IActionResult> GetFollowing(string userId)
    {
        var following = await _mediator.Send(
            new GetFollowingQuery(userId));

        return Ok(following);
    }
}