using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Application.Features.BookReviews.Commands.CreateBookReview;
using WebApi.Application.Features.BookReviews.Commands.DeleteBookReview;
using WebApi.Application.Features.BookReviews.Commands.ToggleBookReviewLike;
using WebApi.Application.Features.BookReviews.Commands.UpdateBookReview;
using WebApi.Application.Features.BookReviews.Queries.GetBookReviewsByBookId;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;
using WebApi.Domain.Enums;

namespace WebApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookReviewsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public BookReviewsController(IMediator mediator, ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(CreateBookReviewCommand command)
    {
        command.UserId = _currentUserService.UserId;

        var reviewId = await _mediator.Send(command);
        return Ok(reviewId);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateBookReviewCommand command)
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
        await _mediator.Send(new DeleteBookReviewCommand(id)
        {
            RequestedByUserId = _currentUserService.UserId
        });

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
            UserId = _currentUserService.UserId
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
            UserId = _currentUserService.UserId
        });

        return Ok(new { Active = result });
    }
}