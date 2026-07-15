using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Interfaces;
using WebApi.Domain.Enum;

namespace WebApi.Application.Features.Social.Commands.RemoveFriend;

public class RemoveFriendCommandHandler
    : IRequestHandler<RemoveFriendCommand, bool>
{
    private readonly IAppDbContext _context;

    public RemoveFriendCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(
        RemoveFriendCommand request,
        CancellationToken cancellationToken)
    {
        var friendship = await _context.Friendships
            .FirstOrDefaultAsync(
                x =>
                    (
                        x.SenderId == request.CurrentUserId &&
                        x.ReceiverId == request.FriendUserId
                    )
                    ||
                    (
                        x.SenderId == request.FriendUserId &&
                        x.ReceiverId == request.CurrentUserId
                    ),
                cancellationToken);

        if (request.CurrentUserId == request.FriendUserId)
            return false;
        
        if (friendship is null)
            return false;

        if (friendship.Status != FriendshipStatus.Accepted)
            return false;

        _context.Friendships.Remove(friendship);

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}