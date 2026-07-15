using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Interfaces;
using WebApi.Domain.Enum;

namespace WebApi.Application.Features.Social.Commands.SendFriendRequest;

public class SendFriendRequestCommandHandler
    : IRequestHandler<SendFriendRequestCommand, bool>
{
    private readonly IAppDbContext _context;

    public SendFriendRequestCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(
        SendFriendRequestCommand request,
        CancellationToken cancellationToken)
    {
        if (request.SenderId == request.ReceiverId)
            return false;

        var exists = await _context.Friendships.AnyAsync(
            x =>
                (x.SenderId == request.SenderId &&
                 x.ReceiverId == request.ReceiverId)
                ||
                (x.SenderId == request.ReceiverId &&
                 x.ReceiverId == request.SenderId),
            cancellationToken);

        if (exists)
            return false;

        var friendship = new Friendship
        {
            SenderId = request.SenderId,
            ReceiverId = request.ReceiverId,
            Status = FriendshipStatus.Pending
        };

        await _context.Friendships.AddAsync(friendship, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}