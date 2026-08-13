using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.Admin.Commands.ToggleUserBan;

public class ToggleUserBanCommandHandler : IRequestHandler<ToggleUserBanCommand, bool>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public ToggleUserBanCommandHandler(IAppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(ToggleUserBanCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
                       .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken)
                   ?? throw new NotFoundException("İstifadəçi tapılmadı.");

        user.IsBanned = !user.IsBanned;

        if (user.IsBanned)
        {
            user.BannedAt = DateTime.UtcNow;
            user.BanReason = request.BanReason;
        }
        else
        {
            user.BannedAt = null;
            user.BanReason = null;
        }

        _context.AdminActivityLogs.Add(new AdminActivityLog
        {
            AdminUsername = _currentUserService.Username ?? "Unknown",
            Action = "BAN_USER",
            Description = user.IsBanned
                ? $"{user.UserName} istifadəçisi bloklandı." + (string.IsNullOrWhiteSpace(request.BanReason) ? "" : $" Səbəb: {request.BanReason}")
                : $"{user.UserName} istifadəçisinin bloku açıldı.",
            TargetEntityType = "User",
            TargetEntityId = Guid.TryParse(user.Id, out var guid) ? guid : null
        });

        await _context.SaveChangesAsync(cancellationToken);

        return user.IsBanned;
    }
}