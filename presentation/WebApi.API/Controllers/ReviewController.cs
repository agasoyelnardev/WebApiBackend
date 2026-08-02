using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Application.Features.Reviews.Commands.CreateReview;
using WebApi.Application.Features.Reviews.Commands.DeleteReview;
using WebApi.Application.Features.Reviews.Commands.ToggleReviewLike;
using WebApi.Application.Features.Reviews.Commands.UpdateReview;
using WebApi.Application.Features.Reviews.Queries.GetReviewsByMovieId;
using WebApi.Application.Interfaces;
using WebApi.Domain.Enums;

namespace WebApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public ReviewsController(IMediator mediator, ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
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
    [HttpPost]
    public async Task<IActionResult> Create(CreateReviewCommand command)
    {
        var reviewId = await _mediator.Send(command);
        return Ok(reviewId);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteReviewCommand(id) { });
        return NoContent();
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateReviewCommand command)
    {
        command.Id = id;
        await _mediator.Send(command);
        return NoContent();
    }

    [Authorize]
    [HttpPost("{id}/like")]
    public async Task<IActionResult> Like(Guid id)
    {
        var result = await _mediator.Send(new ToggleReviewLikeCommand
        {
            ReviewId = id,
            Choice = ReviewLikeChoice.Like,
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
        });

        return Ok(new { Active = result });
    }
}