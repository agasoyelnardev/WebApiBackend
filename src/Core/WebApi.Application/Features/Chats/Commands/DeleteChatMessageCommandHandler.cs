using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Chats.Commands;

public class DeleteChatMessageCommandHandler
    : IRequestHandler<DeleteChatMessageCommand>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public DeleteChatMessageCommandHandler(
        IAppDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task Handle(
        DeleteChatMessageCommand request,
        CancellationToken cancellationToken)
    {
        var message = await _context.ChatMessages
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (message is null)
            throw new Exception("Mesaj tapılmadı.");

        var currentUserId = _currentUserService.UserId;
        var isAdmin = _currentUserService.IsInRole("Admin");

        if (message.UserId != currentUserId && !isAdmin)
        {
            throw new Exception("Bu mesajı silmək hüququnuz yoxdur.");
        }

        _context.ChatMessages.Remove(message);

        await _context.SaveChangesAsync(cancellationToken);
    }
}