using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Enums;

namespace WebApi.Application.Features.Social.Commands.AcceptFriendRequest;

public class AcceptFriendRequestCommandHandler
    : IRequestHandler<AcceptFriendRequestCommand, bool>
{
    private readonly IAppDbContext _context;
    private readonly INotificationService _notificationService;
    private readonly ICurrentUserService _currentUserService;

    public AcceptFriendRequestCommandHandler(
        IAppDbContext context,
        INotificationService notificationService,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _notificationService = notificationService;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(
        AcceptFriendRequestCommand request,
        CancellationToken cancellationToken)
    {
        var friendship = await _context.Friendships
            .FirstOrDefaultAsync(
                x => x.Id == request.FriendshipId,
                cancellationToken);

        var currentUserId = _currentUserService.UserId;

        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");
        
        if (friendship is null)
            throw new NotFoundException("Dostluq sorğusu tapılmadı.");

        if (friendship.ReceiverId != currentUserId)
            throw new UnauthorizedAccessException("Bu sorğunu qəbul etmək icazəniz yoxdur.");

        if (friendship.Status != FriendshipStatus.Pending)
            throw new ConflictException("Bu sorğu artıq emal olunub.");

        friendship.Status = FriendshipStatus.Accepted;
        friendship.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        var accepter = await _context.Users.FirstOrDefaultAsync(
            u => u.Id == currentUserId, cancellationToken);

        await _notificationService.NotifyAsync(
            userId: friendship.SenderId,
            type: "friend_request_accepted",
            title: "Dostluq sorğusu qəbul edildi",
            description: $"{accepter?.UserName ?? "Bir istifadəçi"} dostluq sorğunuzu qəbul etdi.",
            relatedEntityId: friendship.Id,
            cancellationToken: cancellationToken);

        return true;
    }
}