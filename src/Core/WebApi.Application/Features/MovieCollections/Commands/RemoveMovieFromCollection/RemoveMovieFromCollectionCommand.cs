using MediatR;

namespace WebApi.Application.Features.MovieCollections.Commands.RemoveMovieFromCollection;

public class RemoveMovieFromCollectionCommand : IRequest
{
    public Guid MovieCollectionId { get; set; }
    public Guid MovieId { get; set; }

    public string RequestedByUserId { get; set; } = string.Empty;
}