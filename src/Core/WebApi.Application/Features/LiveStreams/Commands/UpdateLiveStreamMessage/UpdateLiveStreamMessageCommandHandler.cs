using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Hubs;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.LiveStreams.Commands.UpdateLiveStreamMessage;

public class UpdateLiveStreamMessageCommandHandler : IRequestHandler<UpdateLiveStreamMessageCommand, Unit>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IHubContext<LiveStreamHub> _hubContext;

    public UpdateLiveStreamMessageCommandHandler(
        IAppDbContext context,
        ICurrentUserService currentUserService,
        IHubContext<LiveStreamHub> hubContext)
    {
        _context = context;
        _currentUserService = currentUserService;
        _hubContext = hubContext;
    }

    public async Task<Unit> Handle(UpdateLiveStreamMessageCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        if (string.IsNullOrWhiteSpace(request.MessageText))
            throw new BadRequestException("Mesaj boş ola bilməz.");

        if (request.MessageText.Length > 500)
            throw new BadRequestException("Mesaj maksimum 500 simvol ola bilər.");

        var message = await _context.LiveStreamMessages
            .FirstOrDefaultAsync(m => m.Id == request.Id && !m.IsDeleted, cancellationToken)
            ?? throw new NotFoundException("Mesaj tapılmadı.");

        var isAdmin = _currentUserService.IsAdmin;
        if (message.UserId != currentUserId && !isAdmin)
            throw new UnauthorizedAccessException("Bu mesajı redaktə etmək hüququnuz yoxdur.");

        message.Message = request.MessageText.Trim();
        message.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        await _hubContext.Clients.Group(message.LiveStreamId.ToString())
            .SendAsync("StreamMessageUpdated", new
            {
                message.Id,
                Message = message.Message
            }, cancellationToken);

        return Unit.Value;
    }
}
