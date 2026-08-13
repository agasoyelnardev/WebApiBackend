using MediatR;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Features.Movies.Commands.CreateMovie;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Movies.Commands.ImportMovieFromTmdb;

public class ImportMovieFromTmdbCommandHandler : IRequestHandler<ImportMovieFromTmdbCommand, Guid>
{
    private readonly IMovieImportService _importService;
    private readonly IMediator _mediator;

    public ImportMovieFromTmdbCommandHandler(IMovieImportService importService, IMediator mediator)
    {
        _importService = importService;
        _mediator = mediator;
    }

    public async Task<Guid> Handle(ImportMovieFromTmdbCommand request, CancellationToken cancellationToken)
    {
        var details = await _importService.GetDetailsAsync(request.TmdbId, cancellationToken);

        if (details is null)
            throw new NotFoundException("TMDB-də bu ID ilə film tapılmadı.");

        // Mövcud CreateMovieCommand-ı çağırırıq — bütün validasiyalar (Title, Year, Genres və s.) avtomatik tətbiq olunur
        var createCommand = new CreateMovieCommand
        {
            Title = details.Title,
            OriginalTitle = details.OriginalTitle,
            Description = details.Description,
            Poster = details.Poster,
            Banner = details.Banner,
            Year = details.Year == 0 ? DateTime.UtcNow.Year : details.Year,
            Duration = string.IsNullOrEmpty(details.Duration) ? "Bilinmir" : details.Duration,
            Director = details.Director,
            TrailerUrl = details.TrailerUrl,
            Genres = details.Genres.Count > 0 ? details.Genres : new List<string> { "Digər" },
            Cast = details.Cast
        };

        return await _mediator.Send(createCommand, cancellationToken);
    }
}