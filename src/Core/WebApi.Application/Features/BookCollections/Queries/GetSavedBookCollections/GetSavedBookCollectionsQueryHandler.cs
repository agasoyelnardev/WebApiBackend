using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Features.BookCollections.Dtos;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.BookCollections.Queries.GetSavedBookCollections;

public class GetSavedBookCollectionsQueryHandler
    : IRequestHandler<GetSavedBookCollectionsQuery, List<BookCollectionDto>>
{
    private readonly IAppDbContext _context;

    public GetSavedBookCollectionsQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<BookCollectionDto>> Handle(
        GetSavedBookCollectionsQuery request, CancellationToken cancellationToken)
    {
        return await _context.SavedBookCollections
            .Where(x => x.UserId == request.UserId)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new BookCollectionDto
            {
                Id = x.BookCollection.Id,
                Title = x.BookCollection.Title,
                Description = x.BookCollection.Description,
                Cover = x.BookCollection.Cover,
                UserId = x.BookCollection.UserId,
                BookCount = x.BookCollection.BookItems.Count
            })
            .ToListAsync(cancellationToken);
    }
}