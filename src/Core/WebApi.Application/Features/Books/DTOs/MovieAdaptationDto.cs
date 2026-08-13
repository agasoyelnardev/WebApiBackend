namespace WebApi.Application.Features.Books.Dtos;

public class MovieAdaptationDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Poster { get; set; } = string.Empty;
    public int Year { get; set; }
}