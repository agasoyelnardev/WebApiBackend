using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResultDto>
{
    private readonly IAppDbContext _context;
    private readonly IJwtService _jwtService;

    public RefreshTokenCommandHandler(IAppDbContext context, IJwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    public async Task<AuthResultDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var storedToken = await _context.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken, cancellationToken);

        if (storedToken is null)
            throw new BadRequestException("Refresh token etibarsızdır.");

        if (storedToken.IsRevoked)
        {
            // Şübhəli fəaliyyət: artıq istifadə olunmuş token yenidən göndərilib.
            // Ehtiyat tədbiri olaraq bu istifadəçinin bütün token-lərini ləğv et.
            var allUserTokens = await _context.RefreshTokens
                .Where(t => t.UserId == storedToken.UserId && !t.IsRevoked)
                .ToListAsync(cancellationToken);

            foreach (var t in allUserTokens)
                t.IsRevoked = true;

            await _context.SaveChangesAsync(cancellationToken);

            throw new UnauthorizedAccessException("Şübhəli fəaliyyət aşkarlandı. Zəhmət olmasa yenidən daxil olun.");
        }

        if (storedToken.IsExpired)
            throw new BadRequestException("Refresh token-in vaxtı bitib.");

        storedToken.IsRevoked = true;

        var newAccessToken = await _jwtService.GenerateToken(storedToken.User);
        var newRefreshTokenValue = _jwtService.GenerateRefreshToken();

        var newRefreshToken = new WebApi.Domain.Entities.RefreshToken
        {
            Token = newRefreshTokenValue,
            UserId = storedToken.UserId,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        _context.RefreshTokens.Add(newRefreshToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new AuthResultDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshTokenValue
        };
    }
}