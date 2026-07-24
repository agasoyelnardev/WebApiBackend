using MediatR;

namespace WebApi.Application.Features.MovieLists.Commands.MarkMovieAsWatched;

public class MarkMovieAsWatchedCommand : IRequest
{
    public Guid MovieId { get; set; }
    public string UserId { get; set; } = string.Empty;
}