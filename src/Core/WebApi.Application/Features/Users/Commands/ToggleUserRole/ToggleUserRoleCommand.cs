using MediatR;

namespace WebApi.Application.Features.Users.Commands.ToggleUserRole;

public class ToggleUserRoleCommand : IRequest
{
    public string UserId { get; set; } = string.Empty;
}