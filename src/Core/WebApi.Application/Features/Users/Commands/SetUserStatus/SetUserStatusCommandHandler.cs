using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Users.Commands.SetUserStatus;

public class SetUserStatusCommandHandler : IRequestHandler<SetUserStatusCommand, bool>
{
    private readonly IAppDbContext _context;
    private readonly IAdminActivityLogger _adminActivityLogger;

    public SetUserStatusCommandHandler(IAppDbContext context, IAdminActivityLogger adminActivityLogger)
    {
        _context = context;
        _adminActivityLogger = adminActivityLogger;
    }

    public async Task<bool> Handle(SetUserStatusCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
        if (user is null)
            throw new NotFoundException("İstifadəçi tapılmadı.");

        user.IsBanned = request.IsBlocked;
        user.BanReason = request.IsBlocked ? request.Reason : null;
        user.BannedAt = request.IsBlocked ? DateTime.UtcNow : null;

        await _context.SaveChangesAsync(cancellationToken);

        await _adminActivityLogger.LogAsync(
            action: request.IsBlocked ? "BanUser" : "UnbanUser",
            description: request.IsBlocked
                ? $"{user.UserName} istifadəçisi bloklandı. Səbəb: {request.Reason ?? "qeyd olunmayıb"}"
                : $"{user.UserName} istifadəçisinin bloku götürüldü.",
            targetEntityId: Guid.TryParse(user.Id, out var uid) ? uid : null,
            targetEntityType: "User",
            cancellationToken: cancellationToken);

        return true;
    }
}