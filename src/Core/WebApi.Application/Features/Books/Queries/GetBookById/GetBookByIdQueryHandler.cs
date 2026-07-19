using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Features.Books.Dtos;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Books.Queries.GetBookById;

public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, BookDto?>
{
    private readonly IAppDbContext _context;

    public GetBookByIdQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<BookDto?> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Books
            .Where(x => x.Id == request.Id && !x.IsDeleted)
            .Select(x => new BookDto
            {
                Id = x.Id,
                Title = x.Title,
                Author = x.Author,
                Description = x.Description,
                Cover = x.Cover,
                Rating = x.Rating,
                Language = x.Language,
                Year = x.Year,
                Pages = x.Pages,
                DownloadUrl = x.DownloadUrl,
                PdfUrl = x.PdfUrl,
                CustomContent = x.CustomContent,
                IsTrending = x.IsTrending,
                IsTopRated = x.IsTopRated,
                IsNewRelease = x.IsNewRelease
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}