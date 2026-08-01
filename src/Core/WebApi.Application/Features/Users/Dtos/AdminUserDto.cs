namespace WebApi.Application.Features.Users.Dtos;

public class AdminUserDto
{
    public string Id { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public bool IsPremium { get; set; }
    public int Points { get; set; }
    public bool IsBlocked { get; set; }
    public string? BanReason { get; set; }
    public DateTime? BannedAt { get; set; }
}