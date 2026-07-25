using MediatR;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Movies.Queries.SearchTmdbMovies;

public class SearchTmdbMoviesQueryHandler : IRequestHandler<SearchTmdbMoviesQuery, List<TmdbMovieSearchResultDto>>
{
    private readonly IMovieImportService _importService;

    public SearchTmdbMoviesQueryHandler(IMovieImportService importService)
    {
        _importService = importService;
    }

    public async Task<List<TmdbMovieSearchResultDto>> Handle(SearchTmdbMoviesQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Query))
            throw new BadRequestException("Axtarış sorğusu boş ola bilməz.");

        return await _importService.SearchAsync(request.Query, cancellationToken);
    }
}