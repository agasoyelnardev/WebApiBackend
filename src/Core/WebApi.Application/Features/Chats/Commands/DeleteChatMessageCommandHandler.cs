using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Hubs;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Chats.Commands;

public class DeleteChatMessageCommandHandler
    : IRequestHandler<DeleteChatMessageCommand>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IHubContext<ChatHub> _hubContext;

    public DeleteChatMessageCommandHandler(
        IAppDbContext context,
        ICurrentUserService currentUserService,
        IHubContext<ChatHub> hubContext)
    {
        _context = context;
        _currentUserService = currentUserService;
        _hubContext = hubContext;
    }

    public async Task Handle(
        DeleteChatMessageCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_currentUserService.UserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        var message = await _context.ChatMessages
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (message is null)
            throw new NotFoundException("Mesaj tapılmadı.");

        var isAdmin = _currentUserService.IsAdmin;

        if (message.UserId != _currentUserService.UserId && !isAdmin)
            throw new UnauthorizedAccessException("Bu mesajı silmək hüququnuz yoxdur.");

        var roomId = message.StreamRoomId;

        _context.ChatMessages.Remove(message);

        await _context.SaveChangesAsync(cancellationToken);

        await _hubContext.Clients.Group(roomId.ToString())
            .SendAsync("MessageDeleted", new { Id = message.Id }, cancellationToken);
    }
}