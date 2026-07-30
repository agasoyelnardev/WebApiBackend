using MediatR;
using WebApi.Application.Features.Movies.Dtos;

namespace WebApi.Application.Features.Movies.Queries.GetFilteredMovies;

public class GetFilteredMoviesQuery : IRequest<List<MovieDto>>
{
    public string? SearchTerm { get; set; }
    public string? Genre { get; set; }        // "Hamsı", "Dram", "Triller" və s.
    public string? YearFilter { get; set; }    // "Hamsı", "2020+", "2010s", "2000s", "Köhnə"
    public string? RatingFilter { get; set; }  // "Hamsı", "8.5", "8.0", "7.5"
    public bool? IsTrending { get; set; }
    public bool? IsTopRated { get; set; }
    public bool? IsNewRelease { get; set; }
    public string? SortBy { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}