namespace WebApi.Application.Features.MovieCollections.Dtos;

public class MovieCollectionDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? CoverImageUrl { get; set; }
    public bool IsPublic { get; set; }
    public string AppUserId { get; set; } = string.Empty;
    public int MovieCount { get; set; }
}