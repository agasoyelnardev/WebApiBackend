namespace WebApi.Application.Features.MovieCollections.Dtos;

public class MovieCollectionDetailDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? CoverImageUrl { get; set; }
    public bool IsPublic { get; set; }
    public string AppUserId { get; set; } = string.Empty;
    public List<MovieSummaryDto> Movies { get; set; } = new();
}