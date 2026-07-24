using MediatR;

namespace WebApi.Application.Features.MovieCollections.Commands.ToggleCollectionLike;

public class ToggleMovieCollectionLikeCommand : IRequest<bool>
{
    public Guid MovieCollectionId { get; set; }
    public string UserId { get; set; } = string.Empty;
}