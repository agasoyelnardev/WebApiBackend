using MediatR;

namespace WebApi.Application.Features.BookLists.Commands.ToggleBookWatchlist;

public class ToggleBookWatchlistCommand : IRequest<bool>
{
    public Guid BookId { get; set; }
    public string UserId { get; set; } = string.Empty;
}