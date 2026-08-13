using MediatR;

namespace WebApi.Application.Features.Users.Commands.DeleteUser;

public record DeleteUserCommand(string UserId) : IRequest;