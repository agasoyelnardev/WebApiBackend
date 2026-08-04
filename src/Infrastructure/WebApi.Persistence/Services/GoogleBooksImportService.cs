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
        var url = $"https://www.googleapis.com/books/v1/volumes?q={Uri.EscapeDataString(query.Trim())}&key={_apiKey}&maxResults=20";        
        HttpResponseMessage? response = null;
        const int maxRetries = 3;

        for (var attempt = 1; attempt <= maxRetries; attempt++)
        {
            response = await _httpClient.GetAsync(url, cancellationToken);

            if (response.IsSuccessStatusCode)
                break;

            // Yalnız müvəqqəti xətalarda (503, 429) yenidən cəhd et
            if (response.StatusCode is System.Net.HttpStatusCode.ServiceUnavailable
                    or (System.Net.HttpStatusCode)429 && attempt < maxRetries)
            {
                await Task.Delay(TimeSpan.FromSeconds(attempt), cancellationToken); // 1s, 2s gözlə
                continue;
            }

            break;
        }

        if (response is null || !response.IsSuccessStatusCode)
        {
            var errorBody = response is not null ? await response.Content.ReadAsStringAsync(cancellationToken) : "Cavab alınmadı";
            throw new InvalidOperationException(
                $"Google Books API xəta qaytardı ({(response is not null ? (int)response.StatusCode : 0)}): {errorBody}");
        }
        
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

        HttpResponseMessage? response = null;
        const int maxRetries = 3;

        for (var attempt = 1; attempt <= maxRetries; attempt++)
        {
            response = await _httpClient.GetAsync(url, cancellationToken);

            if (response.IsSuccessStatusCode)
                break;

            if (response.StatusCode is System.Net.HttpStatusCode.ServiceUnavailable
                    or (System.Net.HttpStatusCode)429 && attempt < maxRetries)
            {
                await Task.Delay(TimeSpan.FromSeconds(attempt), cancellationToken);
                continue;
            }

            break;
        }

        if (response is null)
            return null;

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return null;

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new InvalidOperationException(
                $"Google Books API xəta qaytardı ({(int)response.StatusCode}): {errorBody}");
        }

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

        List<string> genres = new();
        if (volumeInfo.TryGetProperty("categories", out var categoriesArr))
        {
            genres = categoriesArr.EnumerateArray()
                .Select(c => c.GetString())
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Select(c => c!)
                .ToList();
        }
        
        return new GoogleBooksDetailDto
        {
            GoogleBooksId = googleBooksId,
            Title = title,
            Author = author,
            Description = description,
            Cover = cover,
            Year = year,
            Pages = pages,
            PreviewLink = previewLink,
            Genres = genres
        };
    }
}