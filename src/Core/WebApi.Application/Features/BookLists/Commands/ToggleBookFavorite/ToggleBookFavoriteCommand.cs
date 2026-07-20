using MediatR;

namespace WebApi.Application.Features.BookLists.Commands.ToggleBookFavorite;

public class ToggleBookFavoriteCommand : IRequest<bool>
{
    public Guid BookId { get; set; }
    public string UserId { get; set; } = string.Empty;
}