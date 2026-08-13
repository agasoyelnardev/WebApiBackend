using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.Social.Commands.FollowUser;

public class FollowUserCommandHandler
    : IRequestHandler<FollowUserCommand, bool>
{
    private readonly IAppDbContext _context;
    private readonly INotificationService _notificationService;
    private readonly ICurrentUserService _currentUserService;

    public FollowUserCommandHandler(
        IAppDbContext context,
        INotificationService notificationService,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _notificationService = notificationService;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(
        FollowUserCommand request,
        CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;

        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");
        
        if (currentUserId == request.FollowingUserId)
            throw new BadRequestException("Özünüzü izləyə bilməzsiniz.");

        var targetUserExists = await _context.Users.AnyAsync(
            u => u.Id == request.FollowingUserId, cancellationToken);

        if (!targetUserExists)
            throw new NotFoundException("İstifadəçi tapılmadı.");

        var exists = await _context.UserFollows.AnyAsync(
            x => x.FollowerId == currentUserId
                 && x.FollowingId == request.FollowingUserId,
            cancellationToken);

        if (exists)
            throw new ConflictException("Siz artıq bu istifadəçini izləyirsiniz.");

        var follow = new UserFollow
        {
            FollowerId = currentUserId,
            FollowingId = request.FollowingUserId
        };

        await _context.UserFollows.AddAsync(follow, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var follower = await _context.Users.FirstOrDefaultAsync(
            u => u.Id == currentUserId, cancellationToken);

        await _notificationService.NotifyAsync(
            userId: request.FollowingUserId,
            type: "follower",
            title: "Yeni izləyici",
            description: $"{follower?.UserName ?? "Bir istifadəçi"} sizi izləməyə başladı.",
            relatedEntityId: null,
            cancellationToken: cancellationToken);

        return true;
    }
}