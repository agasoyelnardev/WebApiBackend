namespace WebApi.Application.Features.MovieCollections.Dtos;

public class MovieCollectionListItemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? CoverImageUrl { get; set; }
    public string AppUserId { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public int MovieCount { get; set; }
    public int LikesCount { get; set; }
    public DateTime CreatedAt { get; set; }
}