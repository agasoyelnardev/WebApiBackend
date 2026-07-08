using MediatR;

namespace WebApi.Application.Features.Reviews.Commands.CreateReview;

public class CreateReviewCommand : IRequest<Guid>
{
    public string Author { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public double Rating { get; set; }

    public Guid MovieId { get; set; }
}