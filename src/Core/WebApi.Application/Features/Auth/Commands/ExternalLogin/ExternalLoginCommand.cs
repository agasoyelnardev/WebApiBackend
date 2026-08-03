using MediatR;

namespace WebApi.Application.Features.Auth.Commands.ExternalLogin;

public class ExternalLoginCommand : IRequest<AuthResultDto>
{
    public string Provider { get; set; } = string.Empty; // "Google", "Facebook", "Apple"
    public string IdToken { get; set; } = string.Empty;

    public string? FullName { get; set; }
}