using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Reviews.Queries.GetReviewsByMovieId;

public class GetReviewsByMovieIdQueryHandler
    : IRequestHandler<GetReviewsByMovieIdQuery, List<ReviewDto>>
{
    private readonly IAppDbContext _context;

    public GetReviewsByMovieIdQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ReviewDto>> Handle(
        GetReviewsByMovieIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Reviews
            .Where(r => r.MovieId == request.MovieId && !r.IsDeleted)
            .Select(r => new ReviewDto
            {
                Id =  r.Id,
                MovieId = r.MovieId,
                MovieTitle = r.Movie.Title,
                UserId = r.UserId,
                Username = r.User.UserName ?? "Unknown",
                UserAvatar = r.User.Avatar,
                Rating = r.Rating,
                Comment = r.Content,
                Likes = 0,
                Dislikes = 0,
                CreatedAt = r.CreatedAt
            })
            .ToListAsync(cancellationToken);
    }
}
