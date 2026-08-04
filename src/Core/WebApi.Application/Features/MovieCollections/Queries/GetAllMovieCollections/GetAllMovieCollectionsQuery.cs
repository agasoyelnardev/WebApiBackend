using MediatR;
using WebApi.Application.Common.Models;
using WebApi.Application.Features.MovieCollections.Dtos;

namespace WebApi.Application.Features.MovieCollections.Queries.GetAllMovieCollections;

public class GetAllMovieCollectionsQuery : IRequest<PaginatedList<MovieCollectionListItemDto>>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}