using MediatR;

namespace WebApi.Application.Features.BookCollections.Commands.AddBookToCollection;

public class AddBookToCollectionCommand : IRequest
{
    public Guid BookCollectionId { get; set; }
    public Guid BookId { get; set; }

    public string RequestedByUserId { get; set; } = string.Empty;
}