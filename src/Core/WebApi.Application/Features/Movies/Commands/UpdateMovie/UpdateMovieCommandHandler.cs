using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
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
        if (string.IsNullOrWhiteSpace(request.Title))
            throw new BadRequestException("Film adı boş ola bilməz.");

        if (request.Year < 1888 || request.Year > DateTime.UtcNow.Year + 1)
            throw new BadRequestException("Film ili düzgün deyil.");

        if (string.IsNullOrWhiteSpace(request.Duration))
            throw new BadRequestException("Film uzunluğu boş ola bilməz.");

        if (request.Genres is null || !request.Genres.Any())
            throw new BadRequestException("Ən azı bir janr seçilməlidir.");

        var movie = await _context.Movies
            .FirstOrDefaultAsync(
                x => x.Id == request.Id && !x.IsDeleted,
                cancellationToken);

        if (movie is null)
            throw new NotFoundException("Film tapılmadı.");

        movie.Title = request.Title;
        movie.OriginalTitle = request.OriginalTitle;
        movie.Description = request.Description;
        movie.Poster = request.Poster;
        movie.Banner = request.Banner;
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