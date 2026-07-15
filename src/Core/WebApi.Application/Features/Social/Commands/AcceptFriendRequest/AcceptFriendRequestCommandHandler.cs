using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Interfaces;
using WebApi.Domain.Enum;

namespace WebApi.Application.Features.Social.Commands.AcceptFriendRequest;

public class AcceptFriendRequestCommandHandler
    : IRequestHandler<AcceptFriendRequestCommand, bool>
{
    private readonly IAppDbContext _context;

    public AcceptFriendRequestCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(
        AcceptFriendRequestCommand request,
        CancellationToken cancellationToken)
    {
        var friendship = await _context.Friendships
            .FirstOrDefaultAsync(
                x => x.Id == request.FriendshipId,
                cancellationToken);

        if (friendship is null)
            return false;
        
        if (friendship.Status != FriendshipStatus.Pending)
            return false;

        if (friendship.ReceiverId != request.UserId)
            return false;

        friendship.Status = FriendshipStatus.Accepted;
        friendship.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}