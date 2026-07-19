using MediatR;

namespace WebApi.Application.Features.BookReviews.Commands.UpdateBookReview;

public class UpdateBookReviewCommand : IRequest
{
    public Guid Id { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;

    public string RequestedByUserId { get; set; } = string.Empty;
}
