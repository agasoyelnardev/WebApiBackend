using MediatR;
using Microsoft.AspNetCore.Identity;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ICurrentUserService _currentUserService;   

    public DeleteUserCommandHandler(UserManager<AppUser> userManager, ICurrentUserService currentUserService)
    {
        _userManager = userManager;
        _currentUserService = currentUserService;
    }

    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;

        if (request.UserId == currentUserId)   
            throw new BadRequestException("Öz hesabınızı silə bilməzsiniz.");

        var user = await _userManager.FindByIdAsync(request.UserId);

        if (user is null)
            throw new NotFoundException("İstifadəçi tapılmadı.");

        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
            throw new BadRequestException(string.Join(", ", result.Errors.Select(e => e.Description)));
    }
}