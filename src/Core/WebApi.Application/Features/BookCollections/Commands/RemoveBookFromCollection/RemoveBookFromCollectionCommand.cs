using MediatR;

namespace WebApi.Application.Features.BookCollections.Commands.RemoveBookFromCollection;

public class RemoveBookFromCollectionCommand : IRequest
{
    public Guid BookCollectionId { get; set; }
    public Guid BookId { get; set; }

    public string RequestedByUserId { get; set; } = string.Empty;
}