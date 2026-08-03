namespace WebApi.Application.Features.Admin.Dtos;

public class AdminUserDto
{
    public string Id { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
    public bool IsBanned { get; set; }
    public string? BanReason { get; set; }
    public bool IsPremium { get; set; }
    public DateTime? PremiumEndDate { get; set; }
    public int Points { get; set; }
    public DateTime CreatedAt { get; set; }
    public int ReviewCount { get; set; }
}