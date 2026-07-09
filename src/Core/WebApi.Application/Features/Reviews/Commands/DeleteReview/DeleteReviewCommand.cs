using MediatR;

namespace WebApi.Application.Features.Reviews.Commands.DeleteReview;

public class DeleteReviewCommand : IRequest
{
    public Guid Id { get; set; }

    public DeleteReviewCommand(Guid id)
    {
        Id = id;
    }
}