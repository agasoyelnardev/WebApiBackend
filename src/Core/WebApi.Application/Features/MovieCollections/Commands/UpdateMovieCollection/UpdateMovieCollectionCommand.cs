using MediatR;

namespace WebApi.Application.Features.MovieCollections.Commands.UpdateMovieCollection;

public class UpdateMovieCollectionCommand : IRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? CoverImageUrl { get; set; }
    public bool IsPublic { get; set; }

    public string RequestedByUserId { get; set; } = string.Empty;
}