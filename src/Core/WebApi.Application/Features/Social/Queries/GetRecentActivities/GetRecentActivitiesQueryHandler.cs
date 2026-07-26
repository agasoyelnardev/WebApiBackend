using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Features.Social.Dtos;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Social.Queries.GetRecentActivities;

public class GetRecentActivitiesQueryHandler : IRequestHandler<GetRecentActivitiesQuery, List<ActivityDto>>
{
    private readonly IAppDbContext _context;

    public GetRecentActivitiesQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ActivityDto>> Handle(GetRecentActivitiesQuery request, CancellationToken cancellationToken)
    {
        // Son 2 saatın hesablama vaxtı (DateTime.UtcNow - 2 saat)
        var twoHoursAgo = DateTime.UtcNow.AddHours(-request.HoursLimit);

        // Son 2 saatlıq rəylər
        var recentReviews = await _context.Reviews
            .Include(r => r.User)
            .Include(r => r.Movie)
            .Where(r => r.CreatedAt >= twoHoursAgo)
            .OrderByDescending(r => r.CreatedAt)
            .Take(20)
            .Select(r => new ActivityDto
            {
                Id = r.Id.ToString(),
                Type = "review",
                UserId = r.UserId.ToString(),
                Username = r.User.UserName ?? r.User.FullName,
                UserAvatar = r.User.Avatar,
                Text = $"{r.Movie.Title} filminə rəy yazdı: \"{(r.Content.Length > 60 ? r.Content.Substring(0, 60) + "..." : r.Content)}\"",
                MovieId = r.MovieId.ToString(),
                MovieTitle = r.Movie.Title,
                CreatedAt = r.CreatedAt
            })
            .ToListAsync(cancellationToken);

        // Qeyd: Əgər 2 saat ərzində heç bir aktivlik yoxdursa, son 10 aktivliyi fallback olaraq qaytarmaq üçün:
        if (!recentReviews.Any())
        {
            recentReviews = await _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Movie)
                .OrderByDescending(r => r.CreatedAt)
                .Take(10)
                .Select(r => new ActivityDto
                {
                    Id = r.Id.ToString(),
                    Type = "review",
                    UserId = r.UserId.ToString(),
                    Username = r.User.UserName ?? r.User.FullName,
                    UserAvatar = r.User.Avatar,
                    Text = $"{r.Movie.Title} filminə rəy yazdı",
                    MovieId = r.MovieId.ToString(),
                    MovieTitle = r.Movie.Title,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync(cancellationToken);
        }

        return recentReviews;
    }
}