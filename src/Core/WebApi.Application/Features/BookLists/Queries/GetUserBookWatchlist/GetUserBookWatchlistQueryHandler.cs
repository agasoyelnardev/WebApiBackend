using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Features.Books.Dtos;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.BookLists.Queries.GetUserBookWatchlist;

public class GetUserBookWatchlistQueryHandler
    : IRequestHandler<GetUserBookWatchlistQuery, List<BookDto>>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetUserBookWatchlistQueryHandler(IAppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<List<BookDto>> Handle(GetUserBookWatchlistQuery request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;

        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        return await _context.UserBookWatchlistItems
            .Where(x => x.UserId == currentUserId && !x.Book.IsDeleted)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new BookDto
            {
                Id = x.Book.Id,
                Title = x.Book.Title,
                Author = x.Book.Author,
                Description = x.Book.Description,
                Cover = x.Book.Cover,
                Rating = x.Book.Rating,
                Language = x.Book.Language,
                Year = x.Book.Year,
                Pages = x.Book.Pages,
                DownloadUrl = x.Book.DownloadUrl,
                PdfUrl = x.Book.PdfUrl,
                CustomContent = x.Book.CustomContent,
                IsTrending = x.Book.IsTrending,
                IsTopRated = x.Book.IsTopRated,
                IsNewRelease = x.Book.IsNewRelease,
                Likes = x.Book.Likes
            })
            .ToListAsync(cancellationToken);
    }
}