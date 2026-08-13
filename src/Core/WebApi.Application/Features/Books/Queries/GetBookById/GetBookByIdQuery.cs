using MediatR;
using WebApi.Application.Features.Books.Dtos;

namespace WebApi.Application.Features.Books.Queries.GetBookById;

public class GetBookByIdQuery : IRequest<BookDto?>
{
    public Guid Id { get; set; }
    public string? RequestingUserId { get; set; }

    public GetBookByIdQuery(Guid id, string? requestingUserId)
    {
        Id = id;
        RequestingUserId = requestingUserId;
    }
}