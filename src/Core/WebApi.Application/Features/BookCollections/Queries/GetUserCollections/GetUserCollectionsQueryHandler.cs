using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Features.BookCollections.Dtos;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.BookCollections.Queries.GetUserCollections;

public class GetUserCollectionsQueryHandler
    : IRequestHandler<GetUserCollectionsQuery, List<BookCollectionDto>>
{
    private readonly IAppDbContext _context;

    public GetUserCollectionsQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<BookCollectionDto>> Handle(
        GetUserCollectionsQuery request, CancellationToken cancellationToken)
    {
        return await _context.BookCollections
            .Where(c => c.UserId == request.UserId)
            .Select(c => new BookCollectionDto
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                Cover = c.Cover,
                UserId = c.UserId,
                BookCount = c.BookItems.Count
            })
            .ToListAsync(cancellationToken);
    }
}