using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
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
            throw new NotFoundException("Dostluq sorğusu tapılmadı.");

        if (friendship.ReceiverId != request.UserId)
            throw new UnauthorizedAccessException("Bu sorğunu rədd etmək icazəniz yoxdur.");

        if (friendship.Status != FriendshipStatus.Pending)
            throw new ConflictException("Bu sorğu artıq emal olunub.");

        friendship.Status = FriendshipStatus.Declined;
        friendship.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}