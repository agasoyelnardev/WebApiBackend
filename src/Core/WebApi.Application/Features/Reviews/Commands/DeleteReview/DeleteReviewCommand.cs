using MediatR;

namespace WebApi.Application.Features.Reviews.Commands.DeleteReview;

public record DeleteReviewCommand(Guid Id)
    : IRequest<Unit>;