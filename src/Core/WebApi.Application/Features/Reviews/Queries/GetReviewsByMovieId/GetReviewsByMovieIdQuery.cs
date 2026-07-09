using MediatR;

namespace WebApi.Application.Features.Reviews.Queries.GetReviewsByMovieId;

public class GetReviewsByMovieIdQuery : IRequest<List<ReviewDto>>
{
    public Guid MovieId { get; set; }
}