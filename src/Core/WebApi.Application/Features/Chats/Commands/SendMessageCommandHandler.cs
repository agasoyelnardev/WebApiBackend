using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Hubs;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.Chats.Commands;

public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, Guid>
{
    private readonly IChatRepository _repository;
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAppDbContext _context;   

    public SendMessageCommandHandler(
        IChatRepository repository,
        IHubContext<ChatHub> hubContext,
        ICurrentUserService currentUserService,
        IAppDbContext context)
    {
        _repository = repository;
        _hubContext = hubContext;
        _currentUserService = currentUserService;
        _context = context;
    }

    public async Task<Guid> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;

        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        if (string.IsNullOrWhiteSpace(request.MessageText))
            throw new BadRequestException("Mesaj boş ola bilməz.");

        if (request.MessageText.Length > 500)
            throw new BadRequestException("Mesaj maksimum 500 simvol ola bilər.");

        var room = await _repository.GetRoomByIdAsync(request.RoomId);
        if (room is null)
            throw new NotFoundException("Otaq tapılmadı.");

        if (!room.IsLive)
            throw new BadRequestException("Bağlı otağa mesaj göndərilə bilməz.");

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == currentUserId, cancellationToken);
        if (user is null)
            throw new NotFoundException("İstifadəçi tapılmadı.");

        var message = new ChatMessage
        {
            Id = Guid.NewGuid(),
            StreamRoomId = request.RoomId,
            UserId = currentUserId,
            Username = user.UserName ?? "Naməlum",
            UserAvatarUrl = user.Avatar,
            MessageText = request.MessageText,
            IsSystemMessage = false
        };

        await _repository.AddMessageAsync(message);
        await _repository.SaveChangesAsync();

        await _hubContext.Clients.Group(request.RoomId.ToString())
            .SendAsync("ReceiveMessage", new
            {
                message.Id,
                message.UserId,
                message.Username,
                message.UserAvatarUrl,
                message.MessageText
            }, cancellationToken);

        return message.Id;
    }
}