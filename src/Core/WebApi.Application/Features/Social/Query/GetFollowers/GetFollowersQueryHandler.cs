using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Social.Queries.GetFollowers;

public class GetFollowersQueryHandler
    : IRequestHandler<GetFollowersQuery, List<UserPreviewDto>>
{
    private readonly IAppDbContext _context;

    public GetFollowersQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserPreviewDto>> Handle(
        GetFollowersQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.UserFollows
            .Where(x => x.FollowingId == request.UserId)
            .Select(x => new UserPreviewDto(
                x.Follower.Id,
                x.Follower.UserName ?? string.Empty,
                x.Follower.Avatar
            ))
            .ToListAsync(cancellationToken);
    }
}