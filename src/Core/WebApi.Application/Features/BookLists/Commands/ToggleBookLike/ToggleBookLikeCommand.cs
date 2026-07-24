using MediatR;

namespace WebApi.Application.Features.BookLists.Commands.ToggleBookLike;

public class ToggleBookLikeCommand : IRequest<bool>
{
    public Guid BookId { get; set; }
    public string UserId { get; set; } = string.Empty;
}