namespace WebApi.Application.Features.BookCollections.Dtos;

public class BookCollectionDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Cover { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public int BookCount { get; set; }
}