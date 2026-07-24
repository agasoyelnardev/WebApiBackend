using MediatR;

namespace WebApi.Application.Features.BookCollections.Commands.ToggleCollectionLike;

public class ToggleBookCollectionLikeCommand : IRequest<bool>
{
    public Guid BookCollectionId { get; set; }
    public string UserId { get; set; } = string.Empty;
}