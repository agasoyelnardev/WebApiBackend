using MediatR;

namespace WebApi.Application.Features.BookCollections.Commands.CreateBookCollection;

public class CreateBookCollectionCommand : IRequest<Guid>
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Cover { get; set; } = string.Empty;

    public string UserId { get; set; } = string.Empty;
}