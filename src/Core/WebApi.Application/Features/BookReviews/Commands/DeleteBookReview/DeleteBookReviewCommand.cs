using MediatR;

namespace WebApi.Application.Features.BookReviews.Commands.DeleteBookReview;

public record DeleteBookReviewCommand(Guid Id) : IRequest
{
    public string RequestedByUserId { get; set; } = string.Empty;
}