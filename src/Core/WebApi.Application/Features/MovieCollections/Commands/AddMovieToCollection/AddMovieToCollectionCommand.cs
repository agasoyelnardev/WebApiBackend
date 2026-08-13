using MediatR;

namespace WebApi.Application.Features.MovieCollections.Commands.AddMovieToCollection;

public class AddMovieToCollectionCommand : IRequest
{
    public Guid MovieCollectionId { get; set; }
    public Guid MovieId { get; set; }
}