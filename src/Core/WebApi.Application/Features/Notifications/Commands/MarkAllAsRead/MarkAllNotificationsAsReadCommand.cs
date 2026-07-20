using MediatR;

namespace WebApi.Application.Features.Notifications.Commands.MarkAllAsRead;

public class MarkAllNotificationsAsReadCommand : IRequest
{
    public string UserId { get; set; } = string.Empty;
}