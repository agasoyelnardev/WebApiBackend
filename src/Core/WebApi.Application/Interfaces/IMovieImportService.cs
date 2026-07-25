namespace WebApi.Application.Interfaces;

public class TmdbMovieSearchResultDto
{
    public int TmdbId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? PosterUrl { get; set; }
    public string? ReleaseYear { get; set; }
}

public class TmdbMovieDetailDto
{
    public int TmdbId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string OriginalTitle { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Poster { get; set; } = string.Empty;
    public string Banner { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Duration { get; set; } = string.Empty;
    public string Director { get; set; } = string.Empty;
    public List<string> Genres { get; set; } = new();
    public List<string> Cast { get; set; } = new();
    public string TrailerUrl { get; set; } = string.Empty;
}

public interface IMovieImportService
{
    Task<List<TmdbMovieSearchResultDto>> SearchAsync(string query, CancellationToken cancellationToken);
    Task<TmdbMovieDetailDto?> GetDetailsAsync(int tmdbId, CancellationToken cancellationToken);
}