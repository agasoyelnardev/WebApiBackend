using MediatR;
using WebApi.Application.Common.Exceptions;
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
        if (string.IsNullOrWhiteSpace(request.Title))
            throw new BadRequestException("Film adı boş ola bilməz.");

        if (request.Year < 1888 || request.Year > DateTime.UtcNow.Year + 1)
            throw new BadRequestException("Film ili düzgün deyil.");

        if (string.IsNullOrWhiteSpace(request.Duration))
            throw new BadRequestException("Film uzunluğu boş ola bilməz.");

        if (request.Genres is null || !request.Genres.Any())
            throw new BadRequestException("Ən azı bir janr seçilməlidir.");

        var movie = new Movie
        {
            Title = request.Title,
            OriginalTitle = request.OriginalTitle,
            Description = request.Description,
            Poster = request.Poster,
            Banner = request.Banner,
            Rating = 0,
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