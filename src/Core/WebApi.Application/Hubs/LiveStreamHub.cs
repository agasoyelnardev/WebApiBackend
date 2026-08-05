using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using WebApi.Application.Features.LiveStreams.Commands.SendLiveStreamMessage;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Hubs;

[Authorize]
public class LiveStreamHub : Hub
{
    private readonly IMediator _mediator;
    private readonly IAppDbContext _context;
    private readonly ILiveStreamPresenceService _presenceService;

    public LiveStreamHub(IMediator mediator, IAppDbContext context, ILiveStreamPresenceService presenceService)
    {
        _mediator = mediator;
        _context = context;
        _presenceService = presenceService;
    }

    public async Task JoinStreamGroup(Guid streamId)
    {
        var stream = await _context.LiveStreams.FindAsync(streamId);
        if (stream is null)
            throw new HubException("Canlı yayım tapılmadı");

        var groupName = streamId.ToString();

        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        _presenceService.AddConnection(groupName, Context.ConnectionId);

        stream.ViewerCount++;
        await _context.SaveChangesAsync(CancellationToken.None);

        await Clients.Group(groupName).SendAsync("UpdateViewerCount", stream.ViewerCount);
    }

    public async Task LeaveStreamGroup(Guid streamId)
    {
        var groupName = streamId.ToString();

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        _presenceService.RemoveConnection(Context.ConnectionId);

        await DecrementViewerCountAsync(streamId);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var streamIdStr = _presenceService.RemoveConnection(Context.ConnectionId);

        if (streamIdStr is not null && Guid.TryParse(streamIdStr, out var streamId))
        {
            await DecrementViewerCountAsync(streamId);
        }

        await base.OnDisconnectedAsync(exception);
    }

    private async Task DecrementViewerCountAsync(Guid streamId)
    {
        var stream = await _context.LiveStreams.FindAsync(streamId);
        if (stream is null)
            return;

        stream.ViewerCount = Math.Max(0, stream.ViewerCount - 1);
        await _context.SaveChangesAsync(CancellationToken.None);

        await Clients.Group(streamId.ToString()).SendAsync("UpdateViewerCount", stream.ViewerCount);
    }

    public async Task SendStreamMessage(Guid streamId, string message)
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var username = Context.User?.FindFirst(ClaimTypes.Name)?.Value ?? "Anonim";

        var command = new SendLiveStreamMessageCommand
        {
            LiveStreamId = streamId,
            UserId = userId,
            UserName = username,
            UserAvatar = string.Empty, 
            Message = message
        };

        try
        {
            var result = await _mediator.Send(command);
            await Clients.Group(streamId.ToString()).SendAsync("ReceiveStreamMessage", result);
        }
        catch (Exception ex)
        {
            throw new HubException(ex.Message);
        }
    }

    public async Task SendStreamReaction(Guid streamId, string reactionType)
    {
        var allowedReactions = new[] { "heart", "fire", "clap" };

        if (!allowedReactions.Contains(reactionType))
            throw new HubException("Naməlum reaksiya tipi.");

        var username = Context.User?.FindFirst(ClaimTypes.Name)?.Value ?? "Anonim";

        await Clients.Group(streamId.ToString()).SendAsync("ReceiveReaction", new
        {
            ReactionType = reactionType,
            Username = username
        });
    }
}