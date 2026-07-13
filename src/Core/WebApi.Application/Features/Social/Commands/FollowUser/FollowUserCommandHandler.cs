using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.Social.Commands.FollowUser;

public class FollowUserCommandHandler
    : IRequestHandler<FollowUserCommand, bool>
{
    private readonly IAppDbContext _context;

    public FollowUserCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(
        FollowUserCommand request,
        CancellationToken cancellationToken)
    {
        if (request.FollowerUserId == request.FollowingUserId)
            return false;

        var exists = await _context.UserFollows.AnyAsync(
            x => x.FollowerId == request.FollowerUserId
                 && x.FollowingId == request.FollowingUserId,
            cancellationToken);

        if (exists)
            return false;

        var follow = new UserFollow
        {
            FollowerId = request.FollowerUserId,
            FollowingId = request.FollowingUserId
        };

        await _context.UserFollows.AddAsync(follow, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}