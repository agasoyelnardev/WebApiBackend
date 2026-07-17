using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Movies.Queries.GetMovieById;

public class GetMovieByIdQueryHandler
    : IRequestHandler<GetMovieByIdQuery, MovieDto?>
{
    private readonly IAppDbContext _context;

    public GetMovieByIdQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<MovieDto?> Handle(
        GetMovieByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Movies
            .Where(x => x.Id == request.Id && !x.IsDeleted)
            .Select(x => new MovieDto
            {
                Id = x.Id,
                Title = x.Title,
                OriginalTitle = x.OriginalTitle,
                Description = x.Description,
                Poster = x.Poster,
                Banner = x.Banner,
                Rating = x.Rating,
                Year = x.Year,
                Duration = x.Duration,
                Director = x.Director,
                TrailerUrl = x.TrailerUrl,
                VideoUrl = x.VideoUrl,
                Genres = x.Genres,
                Cast = x.Cast
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}