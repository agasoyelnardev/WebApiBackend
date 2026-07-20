using System.Security.Claims;
using System.Collections.Concurrent;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using WebApi.Application.Features.Chats.Commands;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly IMediator _mediator;
    private readonly IChatRepository _repository;

    private static readonly ConcurrentDictionary<string, string> ConnectionRooms = new();

    public ChatHub(IMediator mediator, IChatRepository repository)
    {
        _mediator = mediator;
        _repository = repository;
    }

    public async Task JoinRoom(string roomId)
    {
        if (!Guid.TryParse(roomId, out var roomGuid))
            throw new HubException("Otaq ID formatı yanlışdır");

        var room = await _repository.GetRoomByIdAsync(roomGuid);
        if (room is null)
            throw new HubException("Otaq tapılmadı");

        if (!room.IsLive)
            throw new HubException("Bu otaq bağlıdır");

        await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        ConnectionRooms[Context.ConnectionId] = roomId;

        await _repository.IncrementViewerCountAsync(roomGuid);

        await Clients.Group(roomId).SendAsync("ViewerCountChanged", room.ViewerCount + 1);
    }

    public async Task LeaveRoom(string roomId)
    {
        if (!Guid.TryParse(roomId, out var roomGuid))
            throw new HubException("Otaq ID formatı yanlışdır");

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
        ConnectionRooms.TryRemove(Context.ConnectionId, out _);

        await _repository.DecrementViewerCountAsync(roomGuid);

        var room = await _repository.GetRoomByIdAsync(roomGuid);
        if (room is not null)
            await Clients.Group(roomId).SendAsync("ViewerCountChanged", room.ViewerCount);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (ConnectionRooms.TryRemove(Context.ConnectionId, out var roomId)
            && Guid.TryParse(roomId, out var roomGuid))
        {
            await _repository.DecrementViewerCountAsync(roomGuid);

            var room = await _repository.GetRoomByIdAsync(roomGuid);
            if (room is not null)
                await Clients.Group(roomId).SendAsync("ViewerCountChanged", room.ViewerCount);
        }

        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(string roomId, string messageText)
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var username = Context.User?.FindFirst(ClaimTypes.Name)?.Value ?? "Anonim";

        if (userId is null)
            throw new HubException("İstifadəçi doğrulanmadı");

        if (!Guid.TryParse(roomId, out var roomGuid))
            throw new HubException("Otaq ID formatı yanlışdır");

        var command = new SendMessageCommand(roomGuid, messageText)
        {
            UserId = userId,
            Username = username
        };

        try
        {
            await _mediator.Send(command);
        }
        catch (Exception ex)
        {
            throw new HubException(ex.Message);
        }
    }

    public async Task PlaybackControl(string roomId, string action, double timestamp)
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId is null)
            throw new HubException("İstifadəçi doğrulanmadı");

        if (!Guid.TryParse(roomId, out var roomGuid))
            throw new HubException("Otaq ID formatı yanlışdır");

        var room = await _repository.GetRoomByIdAsync(roomGuid);
        if (room is null)
            throw new HubException("Otaq tapılmadı");

        if (!room.IsLive)
            throw new HubException("Bu otaq bağlıdır");

        if (room.CreatedByUserId != userId)
            throw new HubException("Yalnız otaq sahibi videonu idarə edə bilər");

        // action: "play", "pause", "seek"
        await Clients.OthersInGroup(roomId).SendAsync("PlaybackSync", new
        {
            Action = action,
            Timestamp = timestamp
        });
    }
}