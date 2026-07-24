using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Users.Queries.GetUserProfile;

public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, UserProfileDto?>
{
    private readonly IAppDbContext _context;

    public GetUserProfileQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<UserProfileDto?> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user is null)
            return null;

        var followersCount = await _context.UserFollows
            .CountAsync(f => f.FollowingId == request.UserId, cancellationToken);

        var followingCount = await _context.UserFollows
            .CountAsync(f => f.FollowerId == request.UserId, cancellationToken);

        var badge = user.Points switch
        {
            < 150 => "Kino Həvəskarı",
            >= 150 and < 500 => "Film Tənqidçisi",
            _ => "CineVerse Əfsanəsi"
        };

        return new UserProfileDto
        {
            Id = user.Id,
            UserName = user.UserName ?? string.Empty,
            FullName = user.FullName,
            Avatar = user.Avatar,
            Bio = user.Bio,
            Points = user.Points,
            Badge = badge,
            IsPremium = user.IsPremium,
            FollowersCount = followersCount,
            FollowingCount = followingCount
        };
    }
}