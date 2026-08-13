using MediatR;

namespace WebApi.Application.Features.MovieCollections.Commands.ToggleMovieCollectionLike;

public class ToggleMovieCollectionLikeCommand : IRequest<bool>
{
    public Guid MovieCollectionId { get; set; }
}