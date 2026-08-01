using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Notifications.Commands.DeleteNotification;

public class DeleteNotificationCommandHandler : IRequestHandler<DeleteNotificationCommand>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public DeleteNotificationCommandHandler(IAppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;

        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        var notification = await _context.Notifications
            .FirstOrDefaultAsync(n => n.Id == request.Id, cancellationToken);

        if (notification is null)
            throw new NotFoundException("Bildiriş tapılmadı.");

        if (notification.UserId != currentUserId)
            throw new UnauthorizedAccessException("Bu bildirişi silmək icazəniz yoxdur.");

        _context.Notifications.Remove(notification);
        await _context.SaveChangesAsync(cancellationToken);
    }
}