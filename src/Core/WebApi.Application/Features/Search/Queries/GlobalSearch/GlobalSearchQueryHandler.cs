using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Features.Books.Dtos;
using WebApi.Application.Features.BookCollections.Dtos;
using WebApi.Application.Features.Discussions.Dtos;
using WebApi.Application.Features.MovieCollections.Dtos;
using WebApi.Application.Features.Movies.Dtos;
using WebApi.Application.Features.Movies.Queries.GetMovieById;
using WebApi.Application.Features.Search.Dtos;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Search.Queries.GlobalSearch;

public class GlobalSearchQueryHandler : IRequestHandler<GlobalSearchQuery, GlobalSearchResultDto>
{
    private readonly IAppDbContext _context;

    public GlobalSearchQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<GlobalSearchResultDto> Handle(GlobalSearchQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Query) || request.Query.Trim().Length < 2)
            throw new BadRequestException("Axtarış sorğusu ən azı 2 simvol olmalıdır.");

        var term = request.Query.Trim().ToLower();
        var limit = request.Limit > 20 ? 20 : (request.Limit < 1 ? 5 : request.Limit);

        var movies = await _context.Movies
            .Where(m => !m.IsDeleted && (
                EF.Functions.Like(m.Title, $"%{term}%") ||
                EF.Functions.Like(m.Director, $"%{term}%")))
            .Take(limit)
            .Select(m => new MovieDto
            {
                Id = m.Id,
                Title = m.Title,
                OriginalTitle = m.OriginalTitle,
                Poster = m.Poster,
                Rating = m.Rating,
                Year = m.Year,
                Genres = m.Genres,
                Cast = m.Cast
            })
            .ToListAsync(cancellationToken);

        var books = await _context.Books
            .Where(b => !b.IsDeleted && (
                EF.Functions.Like(b.Title, $"%{term}%") ||
                EF.Functions.Like(b.Author, $"%{term}%")))
            .Take(limit)
            .Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Cover = b.Cover,
                Rating = b.Rating
            })
            .ToListAsync(cancellationToken);

        var users = await _context.Users
            .Where(u => EF.Functions.Like(u.UserName!, $"%{term}%") ||
                        EF.Functions.Like(u.FullName, $"%{term}%"))
            .Take(limit)
            .Select(u => new UserPreviewDto
            {
                Id = u.Id,
                UserName = u.UserName ?? string.Empty,
                Avatar = u.Avatar
            })
            .ToListAsync(cancellationToken);

        var movieCollections = await _context.MovieCollections
            .Where(c => c.IsPublic && EF.Functions.Like(c.Name, $"%{term}%"))
            .Take(limit)
            .Select(c => new MovieCollectionDto
            {
                Id = c.Id,
                Name = c.Name,
                CoverImageUrl = c.CoverImageUrl,
                IsPublic = c.IsPublic,
                AppUserId = c.AppUserId,
                MovieCount = c.Items.Count
            })
            .ToListAsync(cancellationToken);

        var bookCollections = await _context.BookCollections
            .Where(c => EF.Functions.Like(c.Title, $"%{term}%"))
            .Take(limit)
            .Select(c => new BookCollectionDto
            {
                Id = c.Id,
                Title = c.Title,
                Cover = c.Cover,
                UserId = c.UserId,
                BookCount = c.BookItems.Count
            })
            .ToListAsync(cancellationToken);

        var discussions = await _context.Discussions
            .Where(d => EF.Functions.Like(d.Title, $"%{term}%"))
            .Take(limit)
            .Select(d => new DiscussionDto
            {
                Id = d.Id,
                Title = d.Title,
                Category = d.Category.ToString(),
                AuthorId = d.AuthorId,
                Author = d.Author.UserName ?? "Unknown",
                AuthorAvatar = d.Author.Avatar,
                Likes = d.Likes.Count,
                CommentsCount = d.Comments.Count,
                CreatedAt = d.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return new GlobalSearchResultDto
        {
            Movies = movies,
            Books = books,
            Users = users,
            MovieCollections = movieCollections,
            BookCollections = bookCollections,
            Discussions = discussions
        };
    }
}