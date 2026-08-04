using MediatR;
using WebApi.Application.Common.Models;
using WebApi.Application.Features.BookCollections.Dtos;

namespace WebApi.Application.Features.BookCollections.Queries.GetAllBookCollections;

public class GetAllBookCollectionsQuery : IRequest<PaginatedList<BookCollectionListItemDto>>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}