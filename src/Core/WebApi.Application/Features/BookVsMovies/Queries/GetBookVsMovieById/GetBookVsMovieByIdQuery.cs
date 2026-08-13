using MediatR;
using WebApi.Application.Features.BookVsMovies.Dtos;

namespace WebApi.Application.Features.BookVsMovies.Queries.GetBookVsMovieById;

public class GetBookVsMovieByIdQuery : IRequest<BookVsMovieDto?>
{
    public Guid Id { get; set; }
    public string? RequestingUserId { get; set; }

    public GetBookVsMovieByIdQuery(Guid id, string? requestingUserId)
    {
        Id = id;
        RequestingUserId = requestingUserId;
    }
}