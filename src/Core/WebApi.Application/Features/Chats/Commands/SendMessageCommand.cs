using MediatR;
using Microsoft.AspNetCore.SignalR;
using WebApi.Application.Hubs;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.Chats.Commands;

public record SendMessageCommand(Guid RoomId, string MessageText) : IRequest<Guid>
{
    public string UserId { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string UserAvatarUrl { get; set; } = string.Empty;
}

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
        var room = await _repository.GetRoomByIdAsync(request.RoomId);
        if (room is null)
            throw new KeyNotFoundException("Otaq tapılmadı");

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

        // ✅ Real-time yayım — DB-yə yazıldıqdan SONRA
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