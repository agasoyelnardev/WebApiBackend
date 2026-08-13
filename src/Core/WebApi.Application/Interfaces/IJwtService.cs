using WebApi.Domain.Entities;

namespace WebApi.Application.Interfaces;

public interface IJwtService
{
    Task<string> GenerateToken(AppUser user);
    string GenerateRefreshToken();
}