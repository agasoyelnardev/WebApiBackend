using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
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
        if (request.CurrentUserId == request.FriendUserId)
            throw new BadRequestException("Özünüzü dostluqdan silə bilməzsiniz.");

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

        if (friendship is null)
            throw new NotFoundException("Dostluq tapılmadı.");

        if (friendship.Status != FriendshipStatus.Accepted)
            throw new ConflictException("Bu istifadəçi ilə hələ dost deyilsiniz.");

        _context.Friendships.Remove(friendship);

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}