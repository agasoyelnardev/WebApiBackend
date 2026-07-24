using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Application.Features.Reviews.Commands.CreateReview;
using WebApi.Application.Features.Reviews.Commands.DeleteReview;
using WebApi.Application.Features.Reviews.Commands.ToggleReviewLike;
using WebApi.Application.Features.Reviews.Commands.UpdateReview;
using WebApi.Application.Features.Reviews.Queries.GetReviewsByMovieId;
using WebApi.Domain.Entities;
using WebApi.Domain.Enums;
using WebApi.Persistence.Service;

namespace WebApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly CurrentUserService _currentUserService;

    public ReviewsController(IMediator mediator,CurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(CreateReviewCommand command)
    {
        var reviewId = await _mediator.Send(command);

        return Ok(reviewId);
    }
    
    [HttpGet("movie/{movieId}")]
    public async Task<IActionResult> GetByMovieId(Guid movieId)
    {
        var result = await _mediator.Send(
            new GetReviewsByMovieIdQuery()
            {
                MovieId = movieId
            });

        return Ok(result);
    }
    
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteReviewCommand(id));

        return NoContent();
    }
    
    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateReviewCommand command)
    {
        command.Id = id;

        await _mediator.Send(command);

        return NoContent();
    }
    
    [Authorize]
    [HttpPost("{id}/like")]
    public async Task<IActionResult> ToggleLike(Guid id)
    {
        var isLiked = await _mediator.Send(new ToggleReviewLikeCommand
        {
            ReviewId = id,
            UserId = _currentUserService.UserId
        });

        return Ok(new { IsLiked = isLiked });
    }
    
    [Authorize]
    [HttpPost("{id}/like")]
    public async Task<IActionResult> Like(Guid id)
    {
        var result = await _mediator.Send(new ToggleReviewLikeCommand
        {
            ReviewId = id,
            Choice = ReviewLikeChoice.Like,
            UserId = _currentUserService.UserId
        });

        return Ok(new { Active = result });
    }

    [Authorize]
    [HttpPost("{id}/dislike")]
    public async Task<IActionResult> Dislike(Guid id)
    {
        var result = await _mediator.Send(new ToggleReviewLikeCommand
        {
            ReviewId = id,
            Choice = ReviewLikeChoice.Dislike,
            UserId = _currentUserService.UserId
        });

        return Ok(new { Active = result });
    }
}