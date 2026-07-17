using MediatR;
using Microsoft.AspNetCore.SignalR;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Hubs;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.Chats.Commands;

public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, Guid>
{
    private readonly IChatRepository _repository;
    private readonly IHubContext<ChatHub> _hubContext;

    public SendMessageCommandHandler(IChatRepository repository, IHubContext<ChatHub> hubContext)
    {
        _repository = repository;
        _hubContext = hubContext;
    }

    public async Task<Guid> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.UserId))
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

        var message = new ChatMessage
        {
            Id = Guid.NewGuid(),
            StreamRoomId = request.RoomId,
            UserId = request.UserId,
            Username = request.Username,
            UserAvatarUrl = request.UserAvatarUrl,
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