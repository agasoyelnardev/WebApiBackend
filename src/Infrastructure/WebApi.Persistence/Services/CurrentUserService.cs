using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using WebApi.Application.Interfaces;

namespace WebApi.Persistence.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

    public string? Email => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);

    public string? Username => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);

    public bool IsInRole(string role) => _httpContextAccessor.HttpContext?.User?.IsInRole(role) ?? false;

    public bool IsAdmin =>
        IsInRole("Admin")
        || IsInRole("admin")
        || (_httpContextAccessor.HttpContext?.User?.Claims.Any(c =>
            c.Type == ClaimTypes.Role
            && string.Equals(c.Value, "Admin", StringComparison.OrdinalIgnoreCase)) ?? false);
}