using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Notifications.Commands.ToggleRead;

public class ToggleNotificationReadCommandHandler : IRequestHandler<ToggleNotificationReadCommand>
{
    private readonly IAppDbContext _context;

    public ToggleNotificationReadCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(ToggleNotificationReadCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.UserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        var notification = await _context.Notifications
            .FirstOrDefaultAsync(n => n.Id == request.Id, cancellationToken);

        if (notification is null)
            throw new NotFoundException("Bildiriş tapılmadı.");

        if (notification.UserId != request.UserId)
            throw new UnauthorizedAccessException("Bu bildirişə giriş icazəniz yoxdur.");

        notification.IsRead = !notification.IsRead;

        await _context.SaveChangesAsync(cancellationToken);
    }
}