using MediatR;

namespace WebApi.Application.Features.Auth.Commands.Logout;

public class LogoutCommand : IRequest<Unit>
{
    public string RefreshToken { get; set; } = string.Empty;
}