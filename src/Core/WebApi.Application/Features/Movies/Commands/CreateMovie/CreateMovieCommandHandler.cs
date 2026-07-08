using MediatR;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.Movies.Commands.CreateMovie;

public class CreateMovieCommandHandler
    : IRequestHandler<CreateMovieCommand, Guid>
{
    private readonly IAppDbContext _context;

    public CreateMovieCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(
        CreateMovieCommand request,
        CancellationToken cancellationToken)
    {
        var movie = new Movie
        {
            Title = request.Title,
            OriginalTitle = request.OriginalTitle,
            Description = request.Description,
            Poster = request.Poster,
            Banner = request.Banner,
            Rating = request.Rating,
            Year = request.Year,
            Duration = request.Duration,
            Director = request.Director,
            TrailerUrl = request.TrailerUrl,
            VideoUrl = request.VideoUrl,
            Genres = request.Genres,
            Cast = request.Cast
        };

        await _context.Movies.AddAsync(movie, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return movie.Id;
    }
}