
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Persistence.Services;

public class AdminActivityLogger : IAdminActivityLogger
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public AdminActivityLogger(IAppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task LogAsync(
        string action,
        string description,
        Guid? targetEntityId = null,
        string? targetEntityType = null,
        CancellationToken cancellationToken = default)
    {
        var adminId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(adminId))
            return; // sistem hərəkəti kimi loglamırıq, yalnız real admin əməliyyatı

        var log = new AdminActivityLog
        {
            AdminUserId = adminId,
            AdminUsername = _currentUserService.Username ?? "Naməlum",
            Action = action,
            Description = description,
            TargetEntityId = targetEntityId,
            TargetEntityType = targetEntityType
        };

        await _context.AdminActivityLogs.AddAsync(log, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}