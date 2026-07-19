using MediatR;
using WebApi.Application.Features.Books.Dtos;

namespace WebApi.Application.Features.Books.Queries.GetBookById;

public class GetBookByIdQuery : IRequest<BookDto?>
{
    public Guid Id { get; set; }

    public GetBookByIdQuery(Guid id)
    {
        Id = id;
    }
}