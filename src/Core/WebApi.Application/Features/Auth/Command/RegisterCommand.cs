using MediatR;

namespace WebApi.Application.Features.Auth.Commands.Register;

public class RegisterCommand : IRequest<string>
{
    public string FullName { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}