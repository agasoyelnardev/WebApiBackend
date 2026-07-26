using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Interfaces;
using WebApi.Domain.Enum;

namespace WebApi.Application.Features.Social.Query.GetFriends;

public class GetFriendsQueryHandler
    : IRequestHandler<GetFriendsQuery, List<FriendDto>>
{
    private readonly IAppDbContext _context;

    public GetFriendsQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<FriendDto>> Handle(
        GetFriendsQuery request,
        CancellationToken cancellationToken)
    {
        var friendships = await _context.Friendships
            .Where(x =>
                x.Status == FriendshipStatus.Accepted &&
                (x.SenderId == request.UserId ||
                 x.ReceiverId == request.UserId))
            .ToListAsync(cancellationToken);

        var friendIds = friendships
            .Select(x => x.SenderId == request.UserId
                ? x.ReceiverId
                : x.SenderId)
            .ToList();

        return await _context.Users
            .Where(x => friendIds.Contains(x.Id))
            .Select(x => new FriendDto(
                x.Id,
                x.UserName ?? string.Empty,
                x.Avatar
            ))
            .ToListAsync(cancellationToken);
    }
}