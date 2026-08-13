using MediatR;

namespace WebApi.Application.Features.Admin.Commands.DeleteBookReview;

public class DeleteBookReviewCommand : IRequest<Unit>
{
    public Guid BookReviewId { get; set; }
}