using MediatR;
using Microsoft.AspNetCore.Identity;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResultDto>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IJwtService _jwtService;
    private readonly IAppDbContext _context;

    public RegisterCommandHandler(
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IJwtService jwtService,
        IAppDbContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtService = jwtService;
        _context = context;
    }

    public async Task<AuthResultDto> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        request.Email = request.Email.Trim();
        request.UserName = request.UserName.Trim();
        request.FullName = request.FullName?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(request.Email))
            throw new BadRequestException("Email sahəsi boş ola bilməz.");

        if (!System.Text.RegularExpressions.Regex.IsMatch(request.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            throw new BadRequestException("Email formatı düzgün deyil.");

        if (string.IsNullOrWhiteSpace(request.UserName))
            throw new BadRequestException("İstifadəçi adı boş ola bilməz.");

        if (string.IsNullOrWhiteSpace(request.Password))
            throw new BadRequestException("Şifrə boş ola bilməz.");

        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser is not null)
            throw new ConflictException("Bu email artıq istifadə olunur.");

        var existingUsername = await _userManager.FindByNameAsync(request.UserName);
        if (existingUsername is not null)
            throw new ConflictException("Bu istifadəçi adı artıq mövcuddur.");

        var user = new AppUser
        {
            FullName = request.FullName,
            UserName = request.UserName,
            Email = request.Email
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new BadRequestException(errors);
        }

        var roleResult = await _userManager.AddToRoleAsync(user, "User");
        if (!roleResult.Succeeded)
        {
            await _userManager.DeleteAsync(user);
            var roleErrors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
            throw new BadRequestException($"İstifadəçiyə rol təyin edilə bilmədi: {roleErrors}");
        }

        // Qeydiyyatdan keçən kimi avtomatik login (access + refresh token)
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