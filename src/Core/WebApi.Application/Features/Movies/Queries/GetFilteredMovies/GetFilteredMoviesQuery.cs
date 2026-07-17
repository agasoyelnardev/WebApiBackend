using MediatR;
using WebApi.Application.Features.Movies.Queries.GetMovieById;

namespace WebApi.Application.Features.Movies.Queries.GetFilteredMovies;

public class GetFilteredMoviesQuery : IRequest<List<MovieDto>>
{
    public string? SearchTerm { get; set; }
    public string? Genre { get; set; }
    public int? Year { get; set; }
    public double? MinRating { get; set; }
    public string? SortBy { get; set; }

    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}