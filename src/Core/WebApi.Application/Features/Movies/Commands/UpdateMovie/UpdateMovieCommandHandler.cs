using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Movies.Commands.UpdateMovie;

public class UpdateMovieCommandHandler
    : IRequestHandler<UpdateMovieCommand, bool>
{
    private readonly IAppDbContext _context;

    public UpdateMovieCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(
        UpdateMovieCommand request,
        CancellationToken cancellationToken)
    {
        var movie = await _context.Movies
            .FirstOrDefaultAsync(
                x => x.Id == request.Id && !x.IsDeleted,
                cancellationToken);

        if (movie is null)
            return false;

        movie.Title = request.Title;
        movie.OriginalTitle = request.OriginalTitle;
        movie.Description = request.Description;
        movie.Poster = request.Poster;
        movie.Banner = request.Banner;
        movie.Rating = request.Rating;
        movie.Year = request.Year;
        movie.Duration = request.Duration;
        movie.Director = request.Director;
        movie.TrailerUrl = request.TrailerUrl;
        movie.VideoUrl = request.VideoUrl;
        movie.Genres = request.Genres;
        movie.Cast = request.Cast;

        movie.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}