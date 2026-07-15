using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Interfaces;
using WebApi.Domain.Enum;

namespace WebApi.Application.Features.Social.Commands.DeclineFriendRequest;

public class DeclineFriendRequestCommandHandler
    : IRequestHandler<DeclineFriendRequestCommand, bool>
{
    private readonly IAppDbContext _context;

    public DeclineFriendRequestCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(
        DeclineFriendRequestCommand request,
        CancellationToken cancellationToken)
    {
        var friendship = await _context.Friendships
            .FirstOrDefaultAsync(
                x => x.Id == request.FriendshipId,
                cancellationToken);

        if (friendship is null)
            return false;

        if (friendship.ReceiverId != request.UserId)
            return false;

        if (friendship.Status != FriendshipStatus.Pending)
            return false;

        friendship.Status = FriendshipStatus.Declined;
        friendship.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}