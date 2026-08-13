using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Users.Commands.UpdateProfile;

public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateProfileCommandHandler(
        IAppDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;

        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        if (string.IsNullOrWhiteSpace(request.FullName))
            throw new BadRequestException("Ad boş ola bilməz.");

        if (request.FullName.Length > 100)
            throw new BadRequestException("Ad maksimum 100 simvol ola bilər.");

        if (request.Bio.Length > 500)
            throw new BadRequestException("Bio maksimum 500 simvol ola bilər.");

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == currentUserId, cancellationToken);   // ← JWT-dən

        if (user is null)
            throw new NotFoundException("İstifadəçi tapılmadı.");

        user.FullName = request.FullName;
        user.Avatar = request.Avatar;
        user.Bio = request.Bio;

        await _context.SaveChangesAsync(cancellationToken);
    }
}