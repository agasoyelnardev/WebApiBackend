namespace WebApi.Application.Features.Social.Dtos;

public class ActivityDto
{
    public string Id { get; set; } = null!;
    public string Type { get; set; } = null!; // "review", "favorite", "collection", "rate"
    public string UserId { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string? UserAvatar { get; set; }
    public string Text { get; set; } = null!;
    public string? MovieId { get; set; }
    public string? MovieTitle { get; set; }
    public DateTime CreatedAt { get; set; }
}