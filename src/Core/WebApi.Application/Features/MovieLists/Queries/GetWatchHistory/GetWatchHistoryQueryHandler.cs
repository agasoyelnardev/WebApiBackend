using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Features.Movies.Dtos;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.MovieLists.Queries.GetWatchHistory;

public class GetWatchHistoryQueryHandler : IRequestHandler<GetWatchHistoryQuery, List<MovieDto>>
{
    private readonly IAppDbContext _context;

    public GetWatchHistoryQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<MovieDto>> Handle(GetWatchHistoryQuery request, CancellationToken cancellationToken)
    {
        return await _context.WatchHistories
            .Where(x => x.UserId == request.UserId && !x.Movie.IsDeleted)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new MovieDto
            {
                Id = x.Movie.Id,
                Title = x.Movie.Title,
                OriginalTitle = x.Movie.OriginalTitle,
                Poster = x.Movie.Poster,
                Banner = x.Movie.Banner,
                Rating = x.Movie.Rating,
                Year = x.Movie.Year,
                Duration = x.Movie.Duration,
                Director = x.Movie.Director,
                Genres = x.Movie.Genres,
                Cast = x.Movie.Cast,
                Likes = x.Movie.Likes
            })
            .ToListAsync(cancellationToken);
    }
}