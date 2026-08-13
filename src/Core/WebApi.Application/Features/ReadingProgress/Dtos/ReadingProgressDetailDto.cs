namespace WebApi.Application.Features.ReadingProgress.Dtos;

public class ReadingProgressDetailDto
{
    public Guid BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Cover { get; set; } = string.Empty;
    public int Pages { get; set; }
    public int PercentageComplete { get; set; }
    public DateTime? UpdatedAt { get; set; }
}