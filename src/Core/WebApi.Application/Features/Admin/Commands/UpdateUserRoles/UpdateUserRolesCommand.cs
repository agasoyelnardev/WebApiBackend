using MediatR;

namespace WebApi.Application.Features.Admin.Commands.UpdateUserRoles;

public class UpdateUserRolesCommand : IRequest<Unit>
{
    public string UserId { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
}