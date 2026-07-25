using MediatR;

namespace WebApi.Application.Features.Books.Commands.ImportBookFromGoogleBooks;

public class ImportBookFromGoogleBooksCommand : IRequest<Guid>
{
    public string GoogleBooksId { get; set; } = string.Empty;
}