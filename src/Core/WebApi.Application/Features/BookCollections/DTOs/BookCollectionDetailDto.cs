namespace WebApi.Application.Features.BookCollections.Dtos;

public class BookCollectionDetailDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Cover { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public List<BookSummaryDto> Books { get; set; } = new();
}