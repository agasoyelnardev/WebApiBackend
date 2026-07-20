using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Notifications.Commands.MarkAllAsRead;

public class MarkAllNotificationsAsReadCommandHandler : IRequestHandler<MarkAllNotificationsAsReadCommand>
{
    private readonly IAppDbContext _context;

    public MarkAllNotificationsAsReadCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(MarkAllNotificationsAsReadCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.UserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        var unreadNotifications = await _context.Notifications
            .Where(n => n.UserId == request.UserId && !n.IsRead)
            .ToListAsync(cancellationToken);

        foreach (var notification in unreadNotifications)
            notification.IsRead = true;

        if (unreadNotifications.Count > 0)
            await _context.SaveChangesAsync(cancellationToken);
    }
}