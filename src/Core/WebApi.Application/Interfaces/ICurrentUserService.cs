namespace WebApi.Application.Interfaces;

public interface ICurrentUserService
{
    string? UserId { get; }
    string? Email { get; }
    string? Username { get; }
    bool IsInRole(string role);
}