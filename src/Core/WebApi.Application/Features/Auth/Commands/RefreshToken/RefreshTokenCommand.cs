using MediatR;

namespace WebApi.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommand : IRequest<AuthResultDto>
{
    public string RefreshToken { get; set; } = string.Empty;
}