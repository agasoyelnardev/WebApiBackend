using MediatR;
using WebApi.Application.Features.Movies.Dtos;

namespace WebApi.Application.Features.Movies.Queries.GetMovieById;

public class GetMovieByIdQuery : IRequest<MovieDto?>
{
    public Guid Id { get; set; }
    public string? RequestingUserId { get; set; }

    public GetMovieByIdQuery(Guid id, string? requestingUserId)
    {
        Id = id;
        RequestingUserId = requestingUserId;
    }
}