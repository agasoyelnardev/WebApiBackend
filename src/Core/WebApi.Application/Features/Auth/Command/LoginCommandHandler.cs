using MediatR;
using Microsoft.AspNetCore.Identity;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IJwtService _jwtService;

    public LoginCommandHandler(
        UserManager<AppUser> userManager,
        IJwtService jwtService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
    }

    public async Task<string> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
            throw new Exception("Email və ya şifrə yanlışdır.");

        var result = await _userManager.CheckPasswordAsync(
            user,
            request.Password);

        if (!result)
            throw new Exception("Email və ya şifrə yanlışdır.");

        return await _jwtService.GenerateToken(user);
    }
}