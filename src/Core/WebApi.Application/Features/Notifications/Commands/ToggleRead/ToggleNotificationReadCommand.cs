using MediatR;

namespace WebApi.Application.Features.Notifications.Commands.ToggleRead;

public class ToggleNotificationReadCommand : IRequest
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = string.Empty;
}