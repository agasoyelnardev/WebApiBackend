namespace WebApi.Application.Features.Discussions.Dtos;

public class CommentDto
{
    public Guid Id { get; set; }
    public string AuthorId { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string AuthorAvatar { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}