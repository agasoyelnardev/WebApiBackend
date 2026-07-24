namespace WebApi.Application.Features.Books.Dtos;

public class BookDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Cover { get; set; } = string.Empty;
    public double Rating { get; set; }
    public string Language { get; set; } = string.Empty;
    public int Year { get; set; }
    public int Pages { get; set; }
    public string? DownloadUrl { get; set; }
    public string? PdfUrl { get; set; }
    public string? CustomContent { get; set; }
    public bool IsTrending { get; set; }
    public bool IsTopRated { get; set; }
    public bool IsNewRelease { get; set; }
    public List<MovieAdaptationDto> MovieAdaptations { get; set; } = new();
    public int? MyReadingProgress { get; set; }
    public int Likes { get; set; }                    
    public bool IsLikedByCurrentUser { get; set; }
}