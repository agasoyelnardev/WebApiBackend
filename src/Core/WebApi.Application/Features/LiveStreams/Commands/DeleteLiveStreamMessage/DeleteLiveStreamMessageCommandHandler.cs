using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Hubs;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.LiveStreams.Commands.DeleteLiveStreamMessage;

public class DeleteLiveStreamMessageCommandHandler : IRequestHandler<DeleteLiveStreamMessageCommand>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IHubContext<LiveStreamHub> _hubContext;

    public DeleteLiveStreamMessageCommandHandler(
        IAppDbContext context,
        ICurrentUserService currentUserService,
        IHubContext<LiveStreamHub> hubContext)
    {
        _context = context;
        _currentUserService = currentUserService;
        _hubContext = hubContext;
    }

    public async Task Handle(DeleteLiveStreamMessageCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_currentUserService.UserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        var message = await _context.LiveStreamMessages
            .FirstOrDefaultAsync(m => m.Id == request.Id && !m.IsDeleted, cancellationToken)
            ?? throw new NotFoundException("Mesaj tapılmadı.");

        var isAdmin = _currentUserService.IsAdmin;
        if (message.UserId != _currentUserService.UserId && !isAdmin)
            throw new UnauthorizedAccessException("Bu mesajı silmək hüququnuz yoxdur.");

        var streamId = message.LiveStreamId;
        message.IsDeleted = true;
        message.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        await _hubContext.Clients.Group(streamId.ToString())
            .SendAsync("StreamMessageDeleted", new { Id = message.Id }, cancellationToken);
    }
}
