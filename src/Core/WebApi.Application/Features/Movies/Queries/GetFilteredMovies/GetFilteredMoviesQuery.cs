using MediatR;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.Movies.Queries.GetFilteredMovies;

public class GetFilteredMoviesQuery : IRequest<List<Movie>>
{
    public string? SearchTerm { get; set; } 
    public string? Genre { get; set; }      
    public int? Year { get; set; }          
    public double? MinRating { get; set; }  
    public string? SortBy { get; set; }     
}