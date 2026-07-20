using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Social.Commands.FollowUser;

public class FollowUserCommandHandler
    : IRequestHandler<FollowUserCommand, bool>
{
    private readonly IAppDbContext _context;
    private readonly INotificationService _notificationService;

    public FollowUserCommandHandler(IAppDbContext context, INotificationService notificationService)
    {
        _context = context;
        _notificationService = notificationService;
    }

    public async Task<bool> Handle(
        FollowUserCommand request,
        CancellationToken cancellationToken)
    {
        if (request.FollowerUserId == request.FollowingUserId)
            throw new BadRequestException("Özünüzü izləyə bilməzsiniz.");

        var targetUserExists = await _context.Users.AnyAsync(
            u => u.Id == request.FollowingUserId, cancellationToken);

        if (!targetUserExists)
            throw new NotFoundException("İstifadəçi tapılmadı.");

        var exists = await _context.UserFollows.AnyAsync(
            x => x.FollowerId == request.FollowerUserId
                 && x.FollowingId == request.FollowingUserId,
            cancellationToken);

        if (exists)
            throw new ConflictException("Siz artıq bu istifadəçini izləyirsiniz.");

        var follow = new UserFollow
        {
            FollowerId = request.FollowerUserId,
            FollowingId = request.FollowingUserId
        };

        await _context.UserFollows.AddAsync(follow, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var follower = await _context.Users.FirstOrDefaultAsync(
            u => u.Id == request.FollowerUserId, cancellationToken);

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