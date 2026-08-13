using MediatR;
using Microsoft.AspNetCore.Identity;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.Users.Commands.SetUserRole;

public class SetUserRoleCommandHandler : IRequestHandler<SetUserRoleCommand, bool>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IAdminActivityLogger _adminActivityLogger;

    private static readonly string[] AllowedRoles = { "User", "Admin" };

    public SetUserRoleCommandHandler(UserManager<AppUser> userManager, IAdminActivityLogger adminActivityLogger)
    {
        _userManager = userManager;
        _adminActivityLogger = adminActivityLogger;
    }

    public async Task<bool> Handle(SetUserRoleCommand request, CancellationToken cancellationToken)
    {
        if (!AllowedRoles.Contains(request.Role))
            throw new BadRequestException($"'{request.Role}' etibarsız roldur.");

        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user is null)
            throw new NotFoundException("İstifadəçi tapılmadı.");

        var currentRoles = await _userManager.GetRolesAsync(user);

        if (currentRoles.Contains(request.Role))
            return true; 

        var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
        if (!removeResult.Succeeded)
            throw new BadRequestException("Rol dəyişdirilə bilmədi.");

        var addResult = await _userManager.AddToRoleAsync(user, request.Role);
        if (!addResult.Succeeded)
            throw new BadRequestException("Rol dəyişdirilə bilmədi.");

        await _adminActivityLogger.LogAsync(
            action: "SetUserRole",
            description: $"{user.UserName} istifadəçisinin rolu '{request.Role}' olaraq təyin edildi.",
            targetEntityId: Guid.TryParse(user.Id, out var uid) ? uid : null,
            targetEntityType: "User",
            cancellationToken: cancellationToken);

        return true;
    }
}