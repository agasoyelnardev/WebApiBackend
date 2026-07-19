namespace WebApi.Application.Features.BookCollections.Dtos;

public class BookSummaryDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Cover { get; set; } = string.Empty;
    public double Rating { get; set; }
}