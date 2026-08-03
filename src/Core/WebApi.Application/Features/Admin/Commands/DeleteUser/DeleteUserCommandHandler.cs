using MediatR;
using Microsoft.AspNetCore.Identity;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.Admin.Commands.DeleteUser;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public DeleteUserCommandHandler(
        UserManager<AppUser> userManager,
        IAppDbContext context,
        ICurrentUserService currentUserService)
    {
        _userManager = userManager;
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId)
                   ?? throw new NotFoundException("İstifadəçi tapılmadı.");

        var username = user.UserName;

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
            throw new BadRequestException("İstifadəçi silinərkən xəta baş verdi.");

        _context.AdminActivityLogs.Add(new AdminActivityLog
        {
            AdminUsername = _currentUserService.Username ?? "Unknown",
            Action = "DELETE_USER",
            Description = $"{username} istifadəçisi silindi."
        });

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}