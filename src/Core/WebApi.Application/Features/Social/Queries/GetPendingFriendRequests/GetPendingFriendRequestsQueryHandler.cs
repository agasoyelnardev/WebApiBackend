using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Interfaces;
using WebApi.Domain.Enum;

namespace WebApi.Application.Features.Social.Queries.GetPendingFriendRequests;

public class GetPendingFriendRequestsQueryHandler
    : IRequestHandler<GetPendingFriendRequestsQuery, List<FriendRequestDto>>
{
    private readonly IAppDbContext _context;

    public GetPendingFriendRequestsQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<FriendRequestDto>> Handle(
        GetPendingFriendRequestsQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Friendships
            .Where(x =>
                x.ReceiverId == request.UserId &&
                x.Status == FriendshipStatus.Pending)
            .Join(
                _context.Users,
                friendship => friendship.SenderId,
                user => user.Id,
                (friendship, user) => new FriendRequestDto(
                    friendship.Id,
                    user.Id,
                    user.UserName ?? string.Empty,
                    user.Avatar,
                    friendship.CreatedAt
                ))
            .ToListAsync(cancellationToken);
    }
}