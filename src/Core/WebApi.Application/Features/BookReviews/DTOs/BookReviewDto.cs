namespace WebApi.Application.Features.BookReviews.Dtos;

public class BookReviewDto
{
    public Guid Id { get; set; }
    public string Author { get; set; } = string.Empty;
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public int Likes { get; set; }
    public int Dislikes { get; set; }
}