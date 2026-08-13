using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Features.BookCollections.Dtos;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.BookCollections.Queries.GetSavedBookCollections;

public class GetSavedBookCollectionsQueryHandler
    : IRequestHandler<GetSavedBookCollectionsQuery, List<BookCollectionDto>>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetSavedBookCollectionsQueryHandler(IAppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<List<BookCollectionDto>> Handle(
        GetSavedBookCollectionsQuery request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;

        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        return await _context.SavedBookCollections
            .Where(x => x.UserId == currentUserId)
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