using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.Users.Queries.GetCurrentUser;

public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, CurrentUserDto>
{
    private readonly IAppDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public GetCurrentUserQueryHandler(IAppDbContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<CurrentUserDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user is null)
            throw new NotFoundException("İstifadəçi tapılmadı.");

        var roles = await _userManager.GetRolesAsync(user);

        var followersCount = await _context.UserFollows
            .CountAsync(f => f.FollowingId == request.UserId, cancellationToken);

        var followingCount = await _context.UserFollows
            .CountAsync(f => f.FollowerId == request.UserId, cancellationToken);

        var favoriteIds = await _context.UserMovieLists
            .Where(x => x.UserId == request.UserId && x.Type == MovieListType.Favorite)
            .Select(x => x.MovieId)
            .ToListAsync(cancellationToken);

        var watchlistIds = await _context.UserMovieLists
            .Where(x => x.UserId == request.UserId && x.Type == MovieListType.Watchlist)
            .Select(x => x.MovieId)
            .ToListAsync(cancellationToken);
        
        var favoriteBookIds = await _context.UserBookFavorites
            .Where(x => x.UserId == request.UserId)
            .Select(x => x.BookId)
            .ToListAsync(cancellationToken);
        
        var readingProgress = await _context.ReadingProgresses
            .Where(x => x.UserId == request.UserId)
            .ToDictionaryAsync(x => x.BookId, x => x.PercentageComplete, cancellationToken);

        var followingUserIds = await _context.UserFollows
            .Where(f => f.FollowerId == request.UserId)
            .Select(f => f.FollowingId)
            .ToListAsync(cancellationToken);
        
        var bookVotes = await _context.BookVsMovieVotes
            .Where(v => v.UserId == request.UserId)
            .ToDictionaryAsync(v => v.BookVsMovieId, v => v.Choice.ToString(), cancellationToken);
        
        return new CurrentUserDto
        {
            Id = user.Id,
            UserName = user.UserName ?? string.Empty,
            FullName = user.FullName,
            Email = user.Email ?? string.Empty,
            Avatar = user.Avatar,
            Bio = user.Bio,
            Role = roles.FirstOrDefault() ?? "User",
            FollowersCount = followersCount,
            FollowingCount = followingCount,
            FavoriteMovieIds = favoriteIds,
            WatchlistMovieIds = watchlistIds,
            FavoriteBookIds = favoriteBookIds,
            ReadingProgress = readingProgress,
            FollowingUserIds = followingUserIds,
            BookVotes = bookVotes,
            IsPremium = user.IsPremium,
            PremiumEndDate = user.PremiumEndDate
        };
    }
}