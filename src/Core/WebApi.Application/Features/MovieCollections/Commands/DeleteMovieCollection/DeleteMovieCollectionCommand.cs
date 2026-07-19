using MediatR;

namespace WebApi.Application.Features.MovieCollections.Commands.DeleteMovieCollection;

public record DeleteMovieCollectionCommand(Guid Id) : IRequest
{
    public string RequestedByUserId { get; set; } = string.Empty;
}