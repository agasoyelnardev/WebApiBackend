using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Features.Admin.Dtos;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Admin.Queries.GetRecentActivity;

public class GetRecentActivityQueryHandler : IRequestHandler<GetRecentActivityQuery, RecentActivityDto>
{
    private readonly IAppDbContext _context;

    public GetRecentActivityQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<RecentActivityDto> Handle(GetRecentActivityQuery request, CancellationToken cancellationToken)
    {
        var recentUsers = await _context.Users
            .OrderByDescending(u => u.CreatedAt)
            .Take(request.UserCount)
            .Select(u => new RecentUserDto
            {
                Id = u.Id,
                Username = u.UserName ?? string.Empty,
                Email = u.Email ?? string.Empty,
                Avatar = u.Avatar,
                CreatedAt = u.CreatedAt
            })
            .ToListAsync(cancellationToken);

        var recentMovieReviews = await _context.Reviews
            .Where(r => !r.IsDeleted)
            .OrderByDescending(r => r.CreatedAt)
            .Take(request.ReviewCount)
            .Select(r => new RecentReviewDto
            {
                Id = r.Id,
                Type = "Movie",
                TargetTitle = r.Movie.Title,
                Username = r.User.UserName ?? string.Empty,
                Rating = r.Rating,
                Content = r.Content,
                CreatedAt = r.CreatedAt
            })
            .ToListAsync(cancellationToken);

        var recentBookReviews = await _context.BookReviews
            .Where(r => !r.IsDeleted)
            .OrderByDescending(r => r.CreatedAt)
            .Take(request.ReviewCount)
            .Select(r => new RecentReviewDto
            {
                Id = r.Id,
                Type = "Book",
                TargetTitle = r.Book.Title,
                Username = r.User.UserName ?? string.Empty,
                Rating = r.Rating,
                Content = r.Comment,
                CreatedAt = r.CreatedAt
            })
            .ToListAsync(cancellationToken);

        var recentReviews = recentMovieReviews
            .Concat(recentBookReviews)
            .OrderByDescending(r => r.CreatedAt)
            .Take(request.ReviewCount)
            .ToList();

        return new RecentActivityDto
        {
            RecentUsers = recentUsers,
            RecentReviews = recentReviews
        };
    }
}