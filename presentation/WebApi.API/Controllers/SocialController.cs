using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Application.Features.Social.Commands.AcceptFriendRequest;
using WebApi.Application.Features.Social.Commands.DeclineFriendRequest;
using WebApi.Application.Features.Social.Commands.FollowUser;
using WebApi.Application.Features.Social.Commands.RemoveFriend;
using WebApi.Application.Features.Social.Commands.SendFriendRequest;
using WebApi.Application.Features.Social.Commands.UnfollowUser;
using WebApi.Application.Features.Social.Queries.GetFollowers;
using WebApi.Application.Features.Social.Queries.GetFollowing;
using WebApi.Application.Features.Social.Queries.GetPendingFriendRequests;
using WebApi.Application.Features.Social.Query.GetFriends;
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

        await _mediator.Send(
            new FollowUserCommand(userId)
            {
                FollowerUserId = _currentUserService.UserId
            });

        return Ok(new { Message = "İstifadəçi izlənildi" });
    }

    [HttpDelete("follow/{userId}")]
    public async Task<IActionResult> Unfollow(string userId)
    {
        if (_currentUserService.UserId is null)
            return Unauthorized();

        await _mediator.Send(
            new UnfollowUserCommand(userId)
            {
                FollowerUserId = _currentUserService.UserId
            });

        return Ok(new { Message = "İzləmədən çıxarıldı" });
    }

    [AllowAnonymous]
    [HttpGet("followers/{userId}")]
    public async Task<IActionResult> GetFollowers(string userId)
    {
        var followers = await _mediator.Send(new GetFollowersQuery(userId));
        return Ok(followers);
    }

    [AllowAnonymous]
    [HttpGet("following/{userId}")]
    public async Task<IActionResult> GetFollowing(string userId)
    {
        var following = await _mediator.Send(new GetFollowingQuery(userId));
        return Ok(following);
    }
    
    [HttpGet("friends/{userId}")]
    public async Task<IActionResult> GetFriends(string userId)
    {
        var friends = await _mediator.Send(new GetFriendsQuery(userId));
        return Ok(friends);
    }
    
    [HttpGet("friend-requests")]
    public async Task<IActionResult> GetPendingRequests()
    {
        if (_currentUserService.UserId is null)
            return Unauthorized();

        // Təhlükəsizlik: İstifadəçi yalnız özünə gələn sorğuları görə bilər, userId parametrə ehtiyac yoxdur.
        var requests = await _mediator.Send(new GetPendingFriendRequestsQuery(_currentUserService.UserId));
        return Ok(requests);
    }
    
    [HttpPost("friend-request/{userId}")]
    public async Task<IActionResult> SendFriendRequest(string userId)
    {
        if (_currentUserService.UserId is null)
            return Unauthorized();

        await _mediator.Send(
            new SendFriendRequestCommand(userId)
            {
                SenderId = _currentUserService.UserId
            });

        return Ok(new { Message = "Dostluq sorğusu göndərildi" });
    }
    
    [HttpPut("friend-request/{friendshipId}/accept")]
    public async Task<IActionResult> AcceptFriendRequest(Guid friendshipId)
    {
        if (_currentUserService.UserId is null)
            return Unauthorized();

        await _mediator.Send(
            new AcceptFriendRequestCommand(friendshipId)
            {
                UserId = _currentUserService.UserId
            });

        return Ok(new { Message = "Dostluq sorğusu qəbul edildi" });
    }
    
    [HttpPut("friend-request/{friendshipId}/decline")]
    public async Task<IActionResult> DeclineFriendRequest(Guid friendshipId)
    {
        if (_currentUserService.UserId is null)
            return Unauthorized();

        await _mediator.Send(
            new DeclineFriendRequestCommand(friendshipId)
            {
                UserId = _currentUserService.UserId
            });

        return Ok(new { Message = "Dostluq sorğusu rədd edildi" });
    }
    
    [HttpDelete("friend/{userId}")]
    public async Task<IActionResult> RemoveFriend(string userId)
    {
        if (_currentUserService.UserId is null)
            return Unauthorized();

        await _mediator.Send(
            new RemoveFriendCommand(userId)
            {
                CurrentUserId = _currentUserService.UserId
            });

        return Ok(new { Message = "Dostluq silindi" });
    }
}