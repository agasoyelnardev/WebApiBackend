using MediatR;
using WebApi.Application.Features.Movies.Queries.GetMovieById;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.MovieLists.Queries.GetUserMovieList;

public class GetUserMovieListQuery : IRequest<List<MovieDto>>
{
    public string UserId { get; set; } = string.Empty;
    public MovieListType Type { get; set; }
}