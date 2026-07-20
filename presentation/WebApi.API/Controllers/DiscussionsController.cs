using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Application.Features.Discussions.Commands.CreateComment;
using WebApi.Application.Features.Discussions.Commands.CreateDiscussion;
using WebApi.Application.Features.Discussions.Commands.DeleteComment;
using WebApi.Application.Features.Discussions.Commands.DeleteDiscussion;
using WebApi.Application.Features.Discussions.Commands.ToggleDiscussionLike;
using WebApi.Application.Features.Discussions.Commands.UpdateDiscussion;
using WebApi.Application.Features.Discussions.Queries.GetDiscussionById;
using WebApi.Application.Features.Discussions.Queries.GetDiscussions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Enums;

namespace WebApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DiscussionsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public DiscussionsController(IMediator mediator, ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] DiscussionCategory? category,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var result = await _mediator.Send(new GetDiscussionsQuery
        {
            Category = category,
            RequestingUserId = _currentUserService.UserId,
            PageNumber = page,
            PageSize = pageSize
        });

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetDiscussionByIdQuery(id, _currentUserService.UserId));

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(CreateDiscussionCommand command)
    {
        command.AuthorId = _currentUserService.UserId;
        var id = await _mediator.Send(command);
        return Ok(id);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateDiscussionCommand command)
    {
        command.Id = id;
        command.RequestedByUserId = _currentUserService.UserId;
        await _mediator.Send(command);
        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteDiscussionCommand
        {
            Id = id,
            RequestedByUserId = _currentUserService.UserId
        });
        return NoContent();
    }

    [Authorize]
    [HttpPost("{id}/like")]
    public async Task<IActionResult> ToggleLike(Guid id)
    {
        var isLiked = await _mediator.Send(new ToggleDiscussionLikeCommand
        {
            DiscussionId = id,
            UserId = _currentUserService.UserId
        });

        return Ok(new { IsLiked = isLiked });
    }

    [Authorize]
    [HttpPost("{id}/comments")]
    public async Task<IActionResult> AddComment(Guid id, [FromBody] string content)
    {
        var commentId = await _mediator.Send(new CreateCommentCommand
        {
            DiscussionId = id,
            Content = content,
            AuthorId = _currentUserService.UserId
        });

        return Ok(commentId);
    }

    [Authorize]
    [HttpDelete("comments/{commentId}")]
    public async Task<IActionResult> DeleteComment(Guid commentId)
    {
        await _mediator.Send(new DeleteCommentCommand
        {
            Id = commentId,
            RequestedByUserId = _currentUserService.UserId
        });

        return NoContent();
    }
}