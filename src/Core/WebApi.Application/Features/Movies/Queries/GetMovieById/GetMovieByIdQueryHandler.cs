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
                Cast = x.Cast,
                Likes = x.Likes,
                IsLikedByCurrentUser = request.RequestingUserId != null &&
                                       _context.MovieLikes.Any(l => l.MovieId == x.Id && l.UserId == request.RequestingUserId),
                BookSource = x.BookSource != null && !x.BookSource.IsDeleted
                    ? new BookSourceDto
                    {
                        Id = x.BookSource.Id,
                        Title = x.BookSource.Title,
                        Author = x.BookSource.Author,
                        Cover = x.BookSource.Cover
                    }
                    : null
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}