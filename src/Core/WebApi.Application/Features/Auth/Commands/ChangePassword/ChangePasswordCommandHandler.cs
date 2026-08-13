using MediatR;
using Microsoft.AspNetCore.Identity;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.Auth.Commands.ChangePassword;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ICurrentUserService _currentUserService;

    public ChangePasswordCommandHandler(
        UserManager<AppUser> userManager,
        ICurrentUserService currentUserService)
    {
        _userManager = userManager;
        _currentUserService = currentUserService;
    }

    public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;

        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        if (string.IsNullOrWhiteSpace(request.CurrentPassword))
            throw new BadRequestException("Hazırkı şifrə boş ola bilməz.");

        if (string.IsNullOrWhiteSpace(request.NewPassword))
            throw new BadRequestException("Yeni şifrə boş ola bilməz.");

        if (request.NewPassword.Length < 6)
            throw new BadRequestException("Yeni şifrə ən azı 6 simvol olmalıdır.");

        var user = await _userManager.FindByIdAsync(currentUserId);   // ← JWT-dən, request.UserId-dən DEYİL

        if (user is null)
            throw new NotFoundException("İstifadəçi tapılmadı.");

        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

        if (!result.Succeeded)
            throw new BadRequestException(string.Join(", ", result.Errors.Select(e => e.Description)));
    }
}