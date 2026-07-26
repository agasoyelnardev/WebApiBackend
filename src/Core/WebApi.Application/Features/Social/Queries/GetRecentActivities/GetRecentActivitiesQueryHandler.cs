using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Features.Social.Dtos;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.Social.Queries.GetRecentActivities;

public class GetRecentActivitiesQueryHandler : IRequestHandler<GetRecentActivitiesQuery, List<ActivityDto>>
{
    private const int MaxResults = 20;
    private const int FallbackResults = 10;

    private readonly IAppDbContext _context;

    public GetRecentActivitiesQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ActivityDto>> Handle(GetRecentActivitiesQuery request, CancellationToken cancellationToken)
    {
        var since = DateTime.UtcNow.AddHours(-request.HoursLimit);

        var activities = await CollectActivitiesAsync(since, cancellationToken);

        if (activities.Count == 0)
        {
            activities = await CollectActivitiesAsync(DateTime.MinValue, cancellationToken, FallbackResults);
        }

        return activities
            .OrderByDescending(a => a.CreatedAt)
            .Take(MaxResults)
            .ToList();
    }

    private async Task<List<ActivityDto>> CollectActivitiesAsync(
        DateTime since, CancellationToken cancellationToken, int? limitPerType = null)
    {
        var take = limitPerType ?? MaxResults;

        var reviews = await _context.Reviews
            .Where(r => r.CreatedAt >= since)
            .OrderByDescending(r => r.CreatedAt)
            .Take(take)
            .Select(r => new ActivityDto
            {
                Id = r.Id.ToString(),
                Type = "review",
                UserId = r.UserId,
                Username = r.User.UserName ?? r.User.FullName,
                UserAvatar = r.User.Avatar,
                Text = $"{r.Movie.Title} filminə rəy yazdı: \"{(r.Content.Length > 60 ? r.Content.Substring(0, 60) + "..." : r.Content)}\"",
                MovieId = r.MovieId.ToString(),
                MovieTitle = r.Movie.Title,
                CreatedAt = r.CreatedAt
            })
            .ToListAsync(cancellationToken);

        var favorites = await _context.UserMovieLists
            .Where(x => x.Type == MovieListType.Favorite && x.CreatedAt >= since)
            .OrderByDescending(x => x.CreatedAt)
            .Take(take)
            .Select(x => new ActivityDto
            {
                Id = x.Id.ToString(),
                Type = "favorite",
                UserId = x.UserId,
                Username = x.User.UserName ?? x.User.FullName,
                UserAvatar = x.User.Avatar,
                Text = $"{x.Movie.Title} filmini sevimlilərinə əlavə etdi",
                MovieId = x.MovieId.ToString(),
                MovieTitle = x.Movie.Title,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync(cancellationToken);

        var collections = await _context.MovieCollections
            .Where(c => c.IsPublic && c.CreatedAt >= since)
            .OrderByDescending(c => c.CreatedAt)
            .Take(take)
            .Select(c => new ActivityDto
            {
                Id = c.Id.ToString(),
                Type = "collection",
                UserId = c.AppUserId,
                Username = c.AppUser.UserName ?? c.AppUser.FullName,
                UserAvatar = c.AppUser.Avatar,
                Text = $"\"{c.Name}\" adlı yeni kolleksiya yaratdı",
                CreatedAt = c.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return reviews
            .Concat(favorites)
            .Concat(collections)
            .ToList();
    }
}