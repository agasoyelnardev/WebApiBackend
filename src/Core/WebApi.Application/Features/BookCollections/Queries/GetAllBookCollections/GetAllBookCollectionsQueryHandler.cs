using MediatR;
using WebApi.Application.Common.Models;
using WebApi.Application.Features.BookCollections.Dtos;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.BookCollections.Queries.GetAllBookCollections;

public class GetAllBookCollectionsQueryHandler
    : IRequestHandler<GetAllBookCollectionsQuery, PaginatedList<BookCollectionListItemDto>>
{
    private readonly IAppDbContext _context;

    public GetAllBookCollectionsQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<BookCollectionListItemDto>> Handle(
        GetAllBookCollectionsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.BookCollections
            .OrderByDescending(c => c.CreatedAt)
            .Select(c => new BookCollectionListItemDto
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                Cover = c.Cover,
                UserId = c.UserId,
                Username = c.User.UserName ?? string.Empty,
                BooksCount = c.BookItems.Count,
                LikesCount = c.Likes.Count,
                CreatedAt = c.CreatedAt
            });

        return await PaginatedList<BookCollectionListItemDto>.CreateAsync(query, request.Page, request.PageSize);
    }
}