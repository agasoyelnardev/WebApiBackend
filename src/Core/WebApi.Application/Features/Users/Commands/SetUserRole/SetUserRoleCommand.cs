using MediatR;

namespace WebApi.Application.Features.Users.Commands.SetUserRole;

public class SetUserRoleCommand : IRequest<bool>
{
    public string UserId { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty; // "Admin" və ya "User"
}