using MediatR;

namespace WebApi.Application.Features.MovieLists.Commands.ToggleWatchlist;

public class ToggleWatchlistCommand : IRequest<bool>
{
    public Guid MovieId { get; set; }
    public string UserId { get; set; } = string.Empty;
}