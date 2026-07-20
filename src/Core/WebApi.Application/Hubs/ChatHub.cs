using System.Security.Claims;
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
    private readonly IRoomPresenceService _presenceService;

    public ChatHub(IMediator mediator, IChatRepository repository, IRoomPresenceService presenceService)
    {
        _mediator = mediator;
        _repository = repository;
        _presenceService = presenceService;
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

        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId is null)
            throw new HubException("İstifadəçi doğrulanmadı");

        await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        _presenceService.AddConnection(roomId, userId, Context.ConnectionId);

        await _repository.IncrementViewerCountAsync(roomGuid);

        await Clients.Group(roomId).SendAsync("ViewerCountChanged", room.ViewerCount + 1);
    }

    public async Task LeaveRoom(string roomId)
    {
        await HandleLeaveAsync(roomId, Context.ConnectionId);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var roomId = _presenceService.RemoveConnection(Context.ConnectionId, out var userFullyLeft);

        if (roomId is not null && userFullyLeft)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
            await FinalizeLeaveAsync(roomId);
        }

        await base.OnDisconnectedAsync(exception);
    }

    private async Task HandleLeaveAsync(string roomId, string connectionId)
    {
        var removedRoomId = _presenceService.RemoveConnection(connectionId, out var userFullyLeft);

        await Groups.RemoveFromGroupAsync(connectionId, roomId);

        if (userFullyLeft && removedRoomId is not null)
            await FinalizeLeaveAsync(removedRoomId);
    }

    private async Task FinalizeLeaveAsync(string roomId)
    {
        if (!Guid.TryParse(roomId, out var roomGuid))
            return;

        await _repository.DecrementViewerCountAsync(roomGuid);

        var room = await _repository.GetRoomByIdAsync(roomGuid);
        if (room is null)
            return;

        await Clients.Group(roomId).SendAsync("ViewerCountChanged", room.ViewerCount);

        // Əgər çıxan şəxs host idisə, avtomatik yeni host təyin et
        var leavingUserId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (leavingUserId is not null && room.CreatedByUserId == leavingUserId)
        {
            var newHost = _presenceService.GetAnyOtherParticipant(roomId, leavingUserId);

            if (newHost is not null)
            {
                room.CreatedByUserId = newHost;
                await _repository.SaveChangesAsync();

                await Clients.Group(roomId).SendAsync("HostChanged", new { NewHostUserId = newHost });
            }
            // Otaqda heç kim qalmayıbsa, host dəyişmir — otaq "sahibsiz" qalır,
            // sonrakı JoinRoom-larda host hələ də köhnə istifadəçidir (praktiki əhəmiyyəti yoxdur, çünki heç kim yoxdur)
        }
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

        await Clients.OthersInGroup(roomId).SendAsync("PlaybackSync", new
        {
            Action = action,
            Timestamp = timestamp
        });
    }
}