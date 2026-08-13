using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Auth.Commands.Logout;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Unit>
{
    private readonly IAppDbContext _context;

    public LogoutCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        LogoutCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
            throw new BadRequestException("Refresh token boş ola bilməz.");

        var storedToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(
                x => x.Token == request.RefreshToken,
                cancellationToken);

        if (storedToken is null)
            throw new NotFoundException("Refresh token tapılmadı.");

        storedToken.IsRevoked = true;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}