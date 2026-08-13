using MediatR;
using Microsoft.AspNetCore.Identity;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.Admin.Commands.UpdateUserRoles;

public class UpdateUserRolesCommandHandler : IRequestHandler<UpdateUserRolesCommand, Unit>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateUserRolesCommandHandler(
        UserManager<AppUser> userManager,
        IAppDbContext context,
        ICurrentUserService currentUserService)
    {
        _userManager = userManager;
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(UpdateUserRolesCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId)
            ?? throw new NotFoundException("İstifadəçi tapılmadı.");

        var currentRoles = await _userManager.GetRolesAsync(user);

        var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
        if (!removeResult.Succeeded)
            throw new BadRequestException("Köhnə rollar silinərkən xəta baş verdi.");

        var addResult = await _userManager.AddToRolesAsync(user, request.Roles);
        if (!addResult.Succeeded)
            throw new BadRequestException("Yeni rollar əlavə edilərkən xəta baş verdi.");

        _context.AdminActivityLogs.Add(new AdminActivityLog
        {
            AdminUsername = _currentUserService.Username ?? "Unknown",
            Action = "CHANGE_ROLE",
            Description = $"{user.UserName} istifadəçisinin rolları dəyişdirildi: [{string.Join(", ", request.Roles)}]",
            TargetEntityType = "User",
            TargetEntityId = Guid.TryParse(user.Id, out var guid) ? guid : null
        });

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}