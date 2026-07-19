using MediatR;
using WebApi.Application.Features.MovieCollections.Dtos;

namespace WebApi.Application.Features.MovieCollections.Queries.GetUserMovieCollections;

public class GetUserMovieCollectionsQuery : IRequest<List<MovieCollectionDto>>
{
    public string TargetUserId { get; set; } = string.Empty;
    public string? RequestingUserId { get; set; }

    public GetUserMovieCollectionsQuery(string targetUserId, string? requestingUserId)
    {
        TargetUserId = targetUserId;
        RequestingUserId = requestingUserId;
    }
}