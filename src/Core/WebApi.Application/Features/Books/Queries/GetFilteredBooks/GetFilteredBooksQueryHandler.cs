using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Features.Books.Dtos;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.Books.Queries.GetFilteredBooks;

public class GetFilteredBooksQueryHandler : IRequestHandler<GetFilteredBooksQuery, List<BookDto>>
{
    private readonly IAppDbContext _context;

    public GetFilteredBooksQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<BookDto>> Handle(GetFilteredBooksQuery request, CancellationToken cancellationToken)
    {
        var pageSize = request.PageSize > 100 ? 100 : (request.PageSize < 1 ? 20 : request.PageSize);
        var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;

        IQueryable<Book> query = _context.Books
            .Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            string term = request.SearchTerm.Trim().ToLower();
            query = query.Where(b =>
                EF.Functions.Like(b.Title, $"%{term}%") ||
                EF.Functions.Like(b.Author, $"%{term}%"));
        }

        if (!string.IsNullOrWhiteSpace(request.Language))
        {
            query = query.Where(b => b.Language == request.Language);
        }

        if (request.Year.HasValue && request.Year > 0)
        {
            query = query.Where(b => b.Year == request.Year);
        }

        if (request.MinRating.HasValue)
        {
            query = query.Where(b => b.Rating >= request.MinRating);
        }

        if (request.IsTrending.HasValue)
        {
            query = query.Where(b => b.IsTrending == request.IsTrending);
        }

        if (request.IsTopRated.HasValue)
        {
            query = query.Where(b => b.IsTopRated == request.IsTopRated);
        }

        if (request.IsNewRelease.HasValue)
        {
            query = query.Where(b => b.IsNewRelease == request.IsNewRelease);
        }

        query = request.SortBy switch
        {
            "rating_desc" => query.OrderByDescending(b => b.Rating),
            "year_desc" => query.OrderByDescending(b => b.Year),
            "title_asc" => query.OrderBy(b => b.Title),
            _ => query.OrderByDescending(b => b.CreatedAt)
        };

        return await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Description = b.Description,
                Cover = b.Cover,
                Rating = b.Rating,
                Language = b.Language,
                Year = b.Year,
                Pages = b.Pages,
                DownloadUrl = b.DownloadUrl,
                PdfUrl = b.PdfUrl,
                CustomContent = b.CustomContent,
                IsTrending = b.IsTrending,
                IsTopRated = b.IsTopRated,
                IsNewRelease = b.IsNewRelease
            })
            .ToListAsync(cancellationToken);
    }
}