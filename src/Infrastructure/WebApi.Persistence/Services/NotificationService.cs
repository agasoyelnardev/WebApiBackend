using Microsoft.AspNetCore.SignalR;
using WebApi.Application.Hubs;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Services;

public class NotificationService : INotificationService
{
    private readonly IAppDbContext _context;
    private readonly IHubContext<NotificationHub> _hubContext;

    public NotificationService(IAppDbContext context, IHubContext<NotificationHub> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
    }

    public async Task NotifyAsync(
        string userId,
        string type,
        string title,
        string description,
        Guid? relatedEntityId = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(userId))
            return;

        var notification = new Notification
        {
            UserId = userId,
            Type = type,
            Title = title,
            Description = description,
            RelatedEntityId = relatedEntityId
        };

        await _context.Notifications.AddAsync(notification, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        
        await _hubContext.Clients.Group(userId).SendAsync("ReceiveNotification", new
        {
            notification.Id,
            notification.Type,
            notification.Title,
            notification.Description,
            notification.IsRead,
            notification.CreatedAt,
            notification.RelatedEntityId
        }, cancellationToken);
    }
}