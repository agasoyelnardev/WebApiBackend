using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using WebApi.Application.Features.Notifications.Commands.ToggleRead;
using WebApi.Application.Features.Notifications.Queries.GetUnreadNotificationsCount;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Hubs;

[Authorize]
public class NotificationHub : Hub
{
    private readonly IOnlineUsersTracker _onlineUsersTracker;
    private readonly IMediator _mediator;

    public NotificationHub(IOnlineUsersTracker onlineUsersTracker, IMediator mediator)
    {
        _onlineUsersTracker = onlineUsersTracker;
        _mediator = mediator;
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);

            var isFirstConnection = _onlineUsersTracker.AddConnection(userId, Context.ConnectionId);

            var unreadCount = await _mediator.Send(new GetUnreadNotificationsCountQuery { UserId = userId });
            await Clients.Caller.SendAsync("ReceiveUnreadNotificationCount", unreadCount);

            if (isFirstConnection)
                await Clients.Others.SendAsync("UserOnline", userId);

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

            var isLastConnection = _onlineUsersTracker.RemoveConnection(userId, Context.ConnectionId);

            if (isLastConnection)
                await Clients.Others.SendAsync("UserOffline", userId);

            await Clients.All.SendAsync("OnlineCountChanged", _onlineUsersTracker.GetOnlineCount());
        }

        await base.OnDisconnectedAsync(exception);
    }

    public async Task MarkNotificationAsRead(Guid notificationId)
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
            return;

        await _mediator.Send(new ToggleNotificationReadCommand { Id = notificationId, UserId = userId });

        var unreadCount = await _mediator.Send(new GetUnreadNotificationsCountQuery { UserId = userId });
        await Clients.Group(userId).SendAsync("ReceiveUnreadNotificationCount", unreadCount);
    }
}