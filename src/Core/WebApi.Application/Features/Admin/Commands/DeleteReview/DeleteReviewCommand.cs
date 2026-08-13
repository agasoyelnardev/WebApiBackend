using MediatR;

namespace WebApi.Application.Features.Admin.Commands.DeleteReview;

public class DeleteReviewCommand : IRequest<Unit>
{
    public Guid ReviewId { get; set; }
}