namespace WebApi.Application.Interfaces;

public class GoogleBooksSearchResultDto
{
    public string GoogleBooksId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Author { get; set; }
    public string? CoverUrl { get; set; }
}

public class GoogleBooksDetailDto
{
    public string GoogleBooksId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Cover { get; set; } = string.Empty;
    public int Year { get; set; }
    public int Pages { get; set; }
    public string? PreviewLink { get; set; }
}

public interface IBookImportService
{
    Task<List<GoogleBooksSearchResultDto>> SearchAsync(string query, CancellationToken cancellationToken);
    Task<GoogleBooksDetailDto?> GetDetailsAsync(string googleBooksId, CancellationToken cancellationToken);
}