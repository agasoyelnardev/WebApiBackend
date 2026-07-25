using System.Text.Json;
using Microsoft.Extensions.Configuration;
using WebApi.Application.Interfaces;

namespace WebApi.Persistence.Services;

public class GoogleBooksImportService : IBookImportService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public GoogleBooksImportService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["ExternalServices:GoogleBooks:ApiKey"]
            ?? throw new InvalidOperationException("ExternalServices:GoogleBooks:ApiKey təyin edilməyib.");
    }

    public async Task<List<GoogleBooksSearchResultDto>> SearchAsync(string query, CancellationToken cancellationToken)
    {
        var url = $"https://www.googleapis.com/books/v1/volumes?q={Uri.EscapeDataString(query)}&key={_apiKey}&maxResults=20";

        var response = await _httpClient.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        using var doc = JsonDocument.Parse(json);

        var results = new List<GoogleBooksSearchResultDto>();

        if (!doc.RootElement.TryGetProperty("items", out var items))
            return results;

        foreach (var item in items.EnumerateArray())
        {
            var id = item.GetProperty("id").GetString() ?? string.Empty;
            var volumeInfo = item.GetProperty("volumeInfo");

            var title = volumeInfo.TryGetProperty("title", out var t) ? t.GetString() ?? string.Empty : string.Empty;

            string? author = null;
            if (volumeInfo.TryGetProperty("authors", out var authorsArr) && authorsArr.GetArrayLength() > 0)
                author = authorsArr[0].GetString();

            string? coverUrl = null;
            if (volumeInfo.TryGetProperty("imageLinks", out var imageLinks) &&
                imageLinks.TryGetProperty("thumbnail", out var thumb))
                coverUrl = thumb.GetString();

            results.Add(new GoogleBooksSearchResultDto
            {
                GoogleBooksId = id,
                Title = title,
                Author = author,
                CoverUrl = coverUrl
            });
        }

        return results;
    }

    public async Task<GoogleBooksDetailDto?> GetDetailsAsync(string googleBooksId, CancellationToken cancellationToken)
    {
        var url = $"https://www.googleapis.com/books/v1/volumes/{googleBooksId}?key={_apiKey}";

        var response = await _httpClient.GetAsync(url, cancellationToken);
        if (!response.IsSuccessStatusCode)
            return null;

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        var volumeInfo = root.GetProperty("volumeInfo");

        var title = volumeInfo.TryGetProperty("title", out var t) ? t.GetString() ?? string.Empty : string.Empty;

        string author = string.Empty;
        if (volumeInfo.TryGetProperty("authors", out var authorsArr) && authorsArr.GetArrayLength() > 0)
            author = authorsArr[0].GetString() ?? string.Empty;

        var description = volumeInfo.TryGetProperty("description", out var d) ? d.GetString() ?? string.Empty : string.Empty;

        string cover = string.Empty;
        if (volumeInfo.TryGetProperty("imageLinks", out var imageLinks))
        {
            if (imageLinks.TryGetProperty("thumbnail", out var thumb))
                cover = thumb.GetString() ?? string.Empty;
        }

        int year = 0;
        if (volumeInfo.TryGetProperty("publishedDate", out var pd))
        {
            var dateStr = pd.GetString() ?? string.Empty;
            if (dateStr.Length >= 4 && int.TryParse(dateStr[..4], out var parsedYear))
                year = parsedYear;
        }

        int pages = volumeInfo.TryGetProperty("pageCount", out var pc) ? pc.GetInt32() : 0;

        string? previewLink = volumeInfo.TryGetProperty("previewLink", out var pl) ? pl.GetString() : null;

        return new GoogleBooksDetailDto
        {
            GoogleBooksId = googleBooksId,
            Title = title,
            Author = author,
            Description = description,
            Cover = cover,
            Year = year,
            Pages = pages,
            PreviewLink = previewLink
        };
    }
}