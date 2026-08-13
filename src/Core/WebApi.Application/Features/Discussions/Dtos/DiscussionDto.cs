namespace WebApi.Application.Features.Discussions.Dtos;

public class DiscussionDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string AuthorId { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string AuthorAvatar { get; set; } = string.Empty;
    public int Likes { get; set; }
    public bool IsLikedByCurrentUser { get; set; }
    public int CommentsCount { get; set; }
    public DateTime CreatedAt { get; set; }
}