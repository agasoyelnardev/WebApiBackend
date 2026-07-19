using MediatR;

namespace WebApi.Application.Features.BookReviews.Commands.CreateBookReview;

public class CreateBookReviewCommand : IRequest<Guid>
{
    public Guid BookId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;

    public string UserId { get; set; } = string.Empty;
}