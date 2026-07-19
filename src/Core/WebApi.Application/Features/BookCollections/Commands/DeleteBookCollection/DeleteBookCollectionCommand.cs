using MediatR;

namespace WebApi.Application.Features.BookCollections.Commands.DeleteBookCollection;

public record DeleteBookCollectionCommand(Guid Id) : IRequest
{
    public string RequestedByUserId { get; set; } = string.Empty;
}