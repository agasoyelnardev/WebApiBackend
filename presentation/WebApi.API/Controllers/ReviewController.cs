using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Application.Features.Reviews.Commands.CreateReview;

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

    [HttpPost]
    public async Task<IActionResult> Create(CreateReviewCommand command)
    {
        var reviewId = await _mediator.Send(command);

        return Ok(reviewId);
    }
}