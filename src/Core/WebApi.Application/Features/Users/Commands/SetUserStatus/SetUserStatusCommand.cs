using MediatR;

namespace WebApi.Application.Features.Users.Commands.SetUserStatus;

public class SetUserStatusCommand : IRequest<bool>
{
    public string UserId { get; set; } = string.Empty;
    public bool IsBlocked { get; set; }
    public string? Reason { get; set; }
}