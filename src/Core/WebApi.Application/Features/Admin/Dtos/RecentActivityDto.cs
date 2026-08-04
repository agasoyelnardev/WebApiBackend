namespace WebApi.Application.Features.Admin.Dtos;

public class RecentActivityDto
{
    public List<RecentUserDto> RecentUsers { get; set; } = new();
    public List<RecentReviewDto> RecentReviews { get; set; } = new();
}

public class RecentUserDto
{
    public string Id { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class RecentReviewDto
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty; 
    public string TargetTitle { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public double Rating { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}