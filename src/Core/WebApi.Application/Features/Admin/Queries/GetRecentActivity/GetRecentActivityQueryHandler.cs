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
            .OrderByDescending(u => u.Id)
            .Take(10)
            .Select(u => new RecentUserDto
            {
                Id = u.Id,
                Username = u.UserName ?? string.Empty,
                Avatar = u.Avatar,
                CreatedAt = u.CreatedAt
            })
            .ToListAsync(cancellationToken);

        var recentReviews = await _context.Reviews
            .OrderByDescending(r => r.CreatedAt)
            .Take(10)
            .Select(r => new RecentReviewDto
            {
                Id = r.Id,
                Username = r.User.UserName ?? string.Empty,
                MovieTitle = r.Movie.Title,
                Rating = r.Rating,
                Content = r.Content,
                CreatedAt = r.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return new RecentActivityDto
        {
            RecentUsers = recentUsers,
            RecentReviews = recentReviews
        };
    }
}