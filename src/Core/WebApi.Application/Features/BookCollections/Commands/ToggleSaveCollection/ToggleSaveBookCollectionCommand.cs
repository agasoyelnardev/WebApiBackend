using MediatR;

namespace WebApi.Application.Features.BookCollections.Commands.ToggleSaveCollection;

public class ToggleSaveBookCollectionCommand : IRequest<bool>
{
    public Guid BookCollectionId { get; set; }
    public string UserId { get; set; } = string.Empty;
}