using MediatR;

namespace WebApi.Application.Features.Reviews.Commands.UpdateReview;

public class UpdateReviewCommand : IRequest
{
    public Guid Id { get; set; }

    public string Author { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public double Rating { get; set; }
}