using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Enums;

namespace WebApi.Application.Features.Social.Commands.RemoveFriend;

public class RemoveFriendCommandHandler 
    : IRequestHandler<RemoveFriendCommand, bool>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public RemoveFriendCommandHandler(
        IAppDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(
        RemoveFriendCommand request,
        CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;

        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException();


        if (currentUserId == request.FriendUserId)
            throw new BadRequestException(
                "Özünüzü dostluqdan silə bilməzsiniz.");


        var friendship = await _context.Friendships
            .FirstOrDefaultAsync(
                x =>
                    (
                        x.SenderId == currentUserId &&
                        x.ReceiverId == request.FriendUserId
                    )
                    ||
                    (
                        x.SenderId == request.FriendUserId &&
                        x.ReceiverId == currentUserId
                    ),
                cancellationToken);


        if (friendship is null)
            throw new NotFoundException(
                "Dostluq tapılmadı.");


        if (friendship.Status != FriendshipStatus.Accepted)
            throw new ConflictException(
                "Bu istifadəçi ilə hələ dost deyilsiniz.");


        _context.Friendships.Remove(friendship);

        await _context.SaveChangesAsync(cancellationToken);


        return true;
    }
}