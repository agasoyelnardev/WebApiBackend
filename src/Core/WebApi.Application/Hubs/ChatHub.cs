using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using WebApi.Application.Features.Chats.Commands;

namespace WebApi.Application.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly IMediator _mediator;

    public ChatHub(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task JoinRoom(string roomId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
    }

    public async Task LeaveRoom(string roomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
    }

    public async Task SendMessage(string roomId, string messageText)
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var username = Context.User?.FindFirst(ClaimTypes.Name)?.Value ?? "Anonim";

        if (userId is null)
            throw new HubException("İstifadəçi doğrulanmadı");

        var command = new SendMessageCommand(Guid.Parse(roomId), messageText)
        {
            UserId = userId,
            Username = username
        };

        // Handler həm DB-yə yazır, həm də ReceiveMessage ilə broadcast edir
        await _mediator.Send(command);
    }
}