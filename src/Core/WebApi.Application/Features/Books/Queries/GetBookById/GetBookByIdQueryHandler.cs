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
        var book = await _context.Books
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
                IsNewRelease = x.IsNewRelease,
                Likes = x.Likes,
                IsLikedByCurrentUser = request.RequestingUserId != null &&
                    _context.BookLikes.Any(l => l.BookId == x.Id && l.UserId == request.RequestingUserId),
                MovieAdaptations = x.MovieAdaptations
                    .Where(m => !m.IsDeleted)
                    .Select(m => new MovieAdaptationDto
                    {
                        Id = m.Id,
                        Title = m.Title,
                        Poster = m.Poster,
                        Year = m.Year
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (book is null)
            return null;

        if (!string.IsNullOrEmpty(request.RequestingUserId))
        {
            book.MyReadingProgress = await _context.ReadingProgresses
                .Where(p => p.UserId == request.RequestingUserId && p.BookId == request.Id)
                .Select(p => (int?)p.PercentageComplete)
                .FirstOrDefaultAsync(cancellationToken);
        }

        return book;
    }
}