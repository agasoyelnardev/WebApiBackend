using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
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
            throw new BadRequestException("Özünüzə dostluq sorğusu göndərə bilməzsiniz.");

        var receiverExists = await _context.Users.AnyAsync(
            u => u.Id == request.ReceiverId, cancellationToken);

        if (!receiverExists)
            throw new NotFoundException("İstifadəçi tapılmadı.");

        var exists = await _context.Friendships.AnyAsync(
            x =>
                (x.SenderId == request.SenderId &&
                 x.ReceiverId == request.ReceiverId)
                ||
                (x.SenderId == request.ReceiverId &&
                 x.ReceiverId == request.SenderId),
            cancellationToken);

        var existingFriendship = await _context.Friendships.FirstOrDefaultAsync(
            x =>
                (x.SenderId == request.SenderId && x.ReceiverId == request.ReceiverId)
                ||
                (x.SenderId == request.ReceiverId && x.ReceiverId == request.SenderId),
            cancellationToken);

        if (existingFriendship is not null)
        {
            if (existingFriendship.Status == FriendshipStatus.Accepted)
                throw new ConflictException("Siz artıq dostsunuz.");

            throw new ConflictException("Gözləyən dostluq sorğusu artıq mövcuddur.");
        }

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