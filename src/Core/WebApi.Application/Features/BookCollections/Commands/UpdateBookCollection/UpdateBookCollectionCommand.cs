using MediatR;

namespace WebApi.Application.Features.BookCollections.Commands.UpdateBookCollection;

public class UpdateBookCollectionCommand : IRequest
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Cover { get; set; } = string.Empty;

    public string RequestedByUserId { get; set; } = string.Empty;
}