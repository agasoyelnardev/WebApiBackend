using MediatR;
using WebApi.Application.Features.Movies.Dtos;

namespace WebApi.Application.Features.MovieLists.Queries.GetWatchHistory;

public record GetWatchHistoryQuery(string UserId) : IRequest<List<MovieDto>>;