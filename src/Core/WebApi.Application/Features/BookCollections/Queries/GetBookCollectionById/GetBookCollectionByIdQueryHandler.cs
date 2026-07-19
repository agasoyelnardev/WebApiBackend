using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Features.BookCollections.Dtos;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.BookCollections.Queries.GetBookCollectionById;

public class GetBookCollectionByIdQueryHandler
    : IRequestHandler<GetBookCollectionByIdQuery, BookCollectionDetailDto?>
{
    private readonly IAppDbContext _context;

    public GetBookCollectionByIdQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<BookCollectionDetailDto?> Handle(
        GetBookCollectionByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.BookCollections
            .Where(c => c.Id == request.Id)
            .Select(c => new BookCollectionDetailDto
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                Cover = c.Cover,
                UserId = c.UserId,
                Books = c.BookItems
                    .Where(i => !i.Book.IsDeleted)
                    .Select(i => new BookSummaryDto
                    {
                        Id = i.Book.Id,
                        Title = i.Book.Title,
                        Author = i.Book.Author,
                        Cover = i.Book.Cover,
                        Rating = i.Book.Rating
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}