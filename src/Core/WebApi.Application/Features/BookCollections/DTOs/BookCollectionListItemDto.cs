namespace WebApi.Application.Features.BookCollections.Dtos;

public class BookCollectionListItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Cover { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public int BooksCount { get; set; }
    public int LikesCount { get; set; }
    public DateTime CreatedAt { get; set; }
}