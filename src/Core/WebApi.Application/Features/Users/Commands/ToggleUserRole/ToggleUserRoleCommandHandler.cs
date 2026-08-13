using MediatR;
using Microsoft.AspNetCore.Identity;
using WebApi.Application.Common.Exceptions;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.Users.Commands.ToggleUserRole;

public class ToggleUserRoleCommandHandler : IRequestHandler<ToggleUserRoleCommand>
{
    private readonly UserManager<AppUser> _userManager;

    public ToggleUserRoleCommandHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task Handle(ToggleUserRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);

        if (user is null)
            throw new NotFoundException("İstifadəçi tapılmadı.");

        var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

        if (isAdmin)
        {
            await _userManager.RemoveFromRoleAsync(user, "Admin");
            await _userManager.AddToRoleAsync(user, "User");
        }
        else
        {
            await _userManager.RemoveFromRoleAsync(user, "User");
            await _userManager.AddToRoleAsync(user, "Admin");
        }
    }
}