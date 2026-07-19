using MediatR;
using WebApi.Application.Features.MovieCollections.Dtos;

namespace WebApi.Application.Features.MovieCollections.Queries.GetMovieCollectionById;

public class GetMovieCollectionByIdQuery : IRequest<MovieCollectionDetailDto?>
{
    public Guid Id { get; set; }
    public string? RequestingUserId { get; set; }

    public GetMovieCollectionByIdQuery(Guid id, string? requestingUserId)
    {
        Id = id;
        RequestingUserId = requestingUserId;
    }
}