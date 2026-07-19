namespace WebApi.Application.Features.MovieCollections.Dtos;

public class MovieSummaryDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Poster { get; set; } = string.Empty;
    public double Rating { get; set; }
}