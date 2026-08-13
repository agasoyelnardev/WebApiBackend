using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Stats.Queries.GetPublicStats;

public class GetPublicStatsQueryHandler : IRequestHandler<GetPublicStatsQuery, PublicStatsDto>
{
    private readonly IAppDbContext _context;
    private readonly IOnlineUsersTracker _onlineUsersTracker;

    public GetPublicStatsQueryHandler(IAppDbContext context, IOnlineUsersTracker onlineUsersTracker)
    {
        _context = context;
        _onlineUsersTracker = onlineUsersTracker;
    }

    public async Task<PublicStatsDto> Handle(GetPublicStatsQuery request, CancellationToken cancellationToken)
    {
        var totalReviews = await _context.Reviews.CountAsync(cancellationToken)
                           + await _context.BookReviews.CountAsync(cancellationToken);

        var activeRooms = await _context.StreamRooms.CountAsync(r => r.IsLive, cancellationToken);

        return new PublicStatsDto
        {
            OnlineCount = _onlineUsersTracker.GetOnlineCount(),
            TotalReviews = totalReviews,
            ActiveRoomsCount = activeRooms
        };
    }
}