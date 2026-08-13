using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Application.Features.BookReviews.Commands.CreateBookReview;
using WebApi.Application.Features.BookReviews.Commands.DeleteBookReview;
using WebApi.Application.Features.BookReviews.Commands.ToggleBookReviewLike;
using WebApi.Application.Features.BookReviews.Commands.UpdateBookReview;
using WebApi.Application.Features.BookReviews.Queries.GetBookReviewsByBookId;
using WebApi.Domain.Enums;

namespace WebApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookReviewsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BookReviewsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(CreateBookReviewCommand command)
    {
        var reviewId = await _mediator.Send(command);
        return Ok(reviewId);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateBookReviewCommand command)
    {
        command.Id = id;

        await _mediator.Send(command);
        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteBookReviewCommand(id) { });

        return NoContent();
    }

    [HttpGet("book/{bookId}")]
    public async Task<IActionResult> GetByBookId(Guid bookId)
    {
        var reviews = await _mediator.Send(new GetBookReviewsByBookIdQuery(bookId));
        return Ok(reviews);
    }

    [Authorize]
    [HttpPost("{id}/like")]
    public async Task<IActionResult> Like(Guid id)
    {
        var result = await _mediator.Send(new ToggleBookReviewLikeCommand
        {
            BookReviewId = id,
            Choice = ReviewLikeChoice.Like,
        });

        return Ok(new { Active = result });
    }

    [Authorize]
    [HttpPost("{id}/dislike")]
    public async Task<IActionResult> Dislike(Guid id)
    {
        var result = await _mediator.Send(new ToggleBookReviewLikeCommand
        {
            BookReviewId = id,
            Choice = ReviewLikeChoice.Dislike,
        });

        return Ok(new { Active = result });
    }
}