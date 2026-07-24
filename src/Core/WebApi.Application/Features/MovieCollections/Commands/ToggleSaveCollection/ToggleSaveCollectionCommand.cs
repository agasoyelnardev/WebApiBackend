using MediatR;

namespace WebApi.Application.Features.MovieCollections.Commands.ToggleSaveCollection;

public class ToggleSaveCollectionCommand : IRequest<bool>
{
    public Guid MovieCollectionId { get; set; }
    public string UserId { get; set; } = string.Empty;
}