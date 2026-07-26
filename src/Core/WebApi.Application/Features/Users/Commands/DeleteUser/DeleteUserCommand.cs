using MediatR;

namespace WebApi.Application.Features.Users.Commands.DeleteUser;

public class DeleteUserCommand : IRequest
{
    public string UserId { get; set; } = string.Empty;
    public string RequestedByUserId { get; set; } = string.Empty;
}