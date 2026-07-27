using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Hubs;

[Authorize]
public class NotificationHub : Hub
{
    private readonly IOnlineUsersTracker _onlineUsersTracker;

    public NotificationHub(IOnlineUsersTracker onlineUsersTracker)
    {
        _onlineUsersTracker = onlineUsersTracker;
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            _onlineUsersTracker.AddConnection(userId, Context.ConnectionId);

            await Clients.All.SendAsync("OnlineCountChanged", _onlineUsersTracker.GetOnlineCount());
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
        }

        _onlineUsersTracker.RemoveConnection(Context.ConnectionId);

        await Clients.All.SendAsync("OnlineCountChanged", _onlineUsersTracker.GetOnlineCount());

        await base.OnDisconnectedAsync(exception);
    }
}