using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Application.Features.Reviews.Commands.CreateReview;
using WebApi.Application.Features.Reviews.Commands.DeleteReview;
using WebApi.Application.Features.Reviews.Commands.UpdateReview;
using WebApi.Application.Features.Reviews.Queries.GetReviewsByMovieId;

namespace WebApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReviewsController(IMediator mediator)
    {
        _mediator = mediator;
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
}