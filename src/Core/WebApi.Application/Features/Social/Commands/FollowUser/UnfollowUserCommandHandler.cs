using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Social.Commands.UnfollowUser;

public class UnfollowUserCommandHandler
    : IRequestHandler<UnfollowUserCommand, bool>
{
    private readonly IAppDbContext _context;

    public UnfollowUserCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(
        UnfollowUserCommand request,
        CancellationToken cancellationToken)
    {
        var follow = await _context.UserFollows
            .FirstOrDefaultAsync(
                x => x.FollowerId == request.FollowerUserId
                     && x.FollowingId == request.FollowingUserId,
                cancellationToken);

        if (follow is null)
            throw new NotFoundException("İzləmə qeydi tapılmadı.");

        _context.UserFollows.Remove(follow);

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}