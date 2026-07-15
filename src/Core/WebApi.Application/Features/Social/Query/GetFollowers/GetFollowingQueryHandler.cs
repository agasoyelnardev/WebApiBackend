using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Social.Queries.GetFollowing;

public class GetFollowingQueryHandler
    : IRequestHandler<GetFollowingQuery, List<UserPreviewDto>>
{
    private readonly IAppDbContext _context;

    public GetFollowingQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserPreviewDto>> Handle(
        GetFollowingQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.UserFollows
            .Where(x => x.FollowerId == request.UserId)
            .Select(x => new UserPreviewDto(
                x.Following.Id,
                x.Following.UserName ?? string.Empty,
                x.Following.Avatar
            ))
            .ToListAsync(cancellationToken);
    }
}