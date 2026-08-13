using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Social.Commands.UnfollowUser;

public class UnfollowUserCommandHandler
    : IRequestHandler<UnfollowUserCommand, bool>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UnfollowUserCommandHandler(
        IAppDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(
        UnfollowUserCommand request,
        CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;

        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        var follow = await _context.UserFollows
            .FirstOrDefaultAsync(
                x => x.FollowerId == currentUserId &&
                     x.FollowingId == request.FollowingUserId,
                cancellationToken);

        if (follow is null)
            throw new NotFoundException("İzləmə qeydi tapılmadı.");

        _context.UserFollows.Remove(follow);

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}