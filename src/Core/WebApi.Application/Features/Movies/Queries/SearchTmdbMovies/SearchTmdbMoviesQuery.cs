using MediatR;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Movies.Queries.SearchTmdbMovies;

public record SearchTmdbMoviesQuery(string Query) : IRequest<List<TmdbMovieSearchResultDto>>;