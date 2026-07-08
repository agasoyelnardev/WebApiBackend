using MediatR;
using Microsoft.AspNetCore.SignalR; 
using WebApi.Application.Hubs;      
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.Chats.Commands;

public record SendMessageCommand(Guid StreamRoomId, string UserId, string Username, string MessageText) : IRequest<bool>;

public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, bool>
{
    private readonly IChatRepository _chatRepository;
    private readonly IHubContext<ChatHub> _hubContext; 

    public SendMessageCommandHandler(IChatRepository chatRepository, IHubContext<ChatHub> hubContext)
    {
        _chatRepository = chatRepository;
        _hubContext = hubContext;
    }

    public async Task<bool> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        var message = new ChatMessage
        {
            StreamRoomId = request.StreamRoomId,
            UserId = request.UserId,
            Username = request.Username,
            MessageText = request.MessageText
        };

        await _chatRepository.AddMessageAsync(message);
        var isSaved = await _chatRepository.SaveChangesAsync();

        if (isSaved)
        {
            // Otaqdakı hər kəsə anlıq ötürürük
            await _hubContext.Clients.Group(request.StreamRoomId.ToString())
                .SendAsync("ReceiveMessage", request.Username, request.MessageText, DateTime.UtcNow, cancellationToken);
        }

        return isSaved;
    }
}