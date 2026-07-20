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
    private readonly INotificationService _notificationService;

    public SendFriendRequestCommandHandler(IAppDbContext context, INotificationService notificationService)
    {
        _context = context;
        _notificationService = notificationService;
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

        if (exists)
            throw new ConflictException("Artıq dostluq sorğusu mövcuddur, ya da siz artıq dostsunuz.");

        var friendship = new Friendship
        {
            SenderId = request.SenderId,
            ReceiverId = request.ReceiverId,
            Status = FriendshipStatus.Pending
        };

        await _context.Friendships.AddAsync(friendship, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var sender = await _context.Users.FirstOrDefaultAsync(
            u => u.Id == request.SenderId, cancellationToken);

        await _notificationService.NotifyAsync(
            userId: request.ReceiverId,
            type: "friend_request",
            title: "Yeni dostluq sorğusu",
            description: $"{sender?.UserName ?? "Bir istifadəçi"} sizə dostluq sorğusu göndərdi.",
            relatedEntityId: friendship.Id,
            cancellationToken: cancellationToken);

        return true;
    }
}