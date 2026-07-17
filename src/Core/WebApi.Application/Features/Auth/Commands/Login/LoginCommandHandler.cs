using MediatR;
using Microsoft.AspNetCore.Identity;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;
using WebApi.Application.Common.Exceptions;

namespace WebApi.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResultDto>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IJwtService _jwtService;
    private readonly IAppDbContext _context;

    public LoginCommandHandler(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IJwtService jwtService,
        IAppDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtService = jwtService;
        _context = context;
    }

    public async Task<AuthResultDto> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
            throw new BadRequestException("Email boş ola bilməz.");

        if (string.IsNullOrWhiteSpace(request.Password))
            throw new BadRequestException("Şifrə boş ola bilməz.");

        var user = await _userManager.FindByEmailAsync(request.Email.Trim());

        if (user is null)
            throw new BadRequestException("Email və ya şifrə yanlışdır.");

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);

        if (result.IsLockedOut)
            throw new BadRequestException("Hesabınız çoxlu yanlış cəhd səbəbindən müvəqqəti bloklanıb. Zəhmət olmasa, bir az sonra yenidən cəhd edin.");

        if (!result.Succeeded)
            throw new BadRequestException("Email və ya şifrə yanlışdır.");

        var accessToken = await _jwtService.GenerateToken(user);
        var refreshTokenValue = _jwtService.GenerateRefreshToken();

        var refreshToken = new Domain.Entities.RefreshToken
        {
            Token = refreshTokenValue,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new AuthResultDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshTokenValue
        };
    }
}