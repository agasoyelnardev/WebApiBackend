using MediatR;

namespace WebApi.Application.Features.MovieCollections.Commands.CreateMovieCollection;

public class CreateMovieCollectionCommand : IRequest<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? CoverImageUrl { get; set; }
    public bool IsPublic { get; set; } = true;

    public string AppUserId { get; set; } = string.Empty;
}