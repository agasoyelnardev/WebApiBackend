namespace WebApi.Application.Features.Movies.Dtos;

public class BookSourceDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Cover { get; set; } = string.Empty;
}