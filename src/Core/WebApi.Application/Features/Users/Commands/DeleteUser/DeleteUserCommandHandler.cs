using MediatR;
using Microsoft.AspNetCore.Identity;
using WebApi.Application.Common.Exceptions;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly UserManager<AppUser> _userManager;

    public DeleteUserCommandHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        if (request.UserId == request.RequestedByUserId)
            throw new BadRequestException("Öz hesabınızı silə bilməzsiniz.");

        var user = await _userManager.FindByIdAsync(request.UserId);

        if (user is null)
            throw new NotFoundException("İstifadəçi tapılmadı.");

        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
            throw new BadRequestException(string.Join(", ", result.Errors.Select(e => e.Description)));
    }
}