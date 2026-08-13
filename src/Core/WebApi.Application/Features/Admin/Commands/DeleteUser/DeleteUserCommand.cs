using MediatR;

namespace WebApi.Application.Features.Admin.Commands.DeleteUser;

public class DeleteUserCommand : IRequest<Unit>
{
    public string UserId { get; set; } = string.Empty;
}