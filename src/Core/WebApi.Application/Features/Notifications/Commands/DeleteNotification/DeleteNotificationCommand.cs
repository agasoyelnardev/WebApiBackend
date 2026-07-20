using MediatR;

namespace WebApi.Application.Features.Notifications.Commands.DeleteNotification;

public class DeleteNotificationCommand : IRequest
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = string.Empty;
}