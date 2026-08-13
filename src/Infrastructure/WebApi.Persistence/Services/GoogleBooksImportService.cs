using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;

namespace WebApi.Persistence.Services;

public class GoogleBooksImportService : IBookImportService
{
    private readonly HttpClient _httpClient;
    private readonly string? _apiKey;

    public GoogleBooksImportService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["ExternalServices:GoogleBooks:ApiKey"];
    }

    public async Task<List<GoogleBooksSearchResultDto>> SearchAsync(string query, CancellationToken cancellationToken)
    {
        var url = BuildVolumesUrl(query);
        var json = await GetGoogleBooksJsonAsync(url, cancellationToken);

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
        var url = BuildVolumeUrl(googleBooksId);

        HttpResponseMessage? response = null;
        const int maxRetries = 4;

        for (var attempt = 1; attempt <= maxRetries; attempt++)
        {
            response = await _httpClient.GetAsync(url, cancellationToken);

            if (response.IsSuccessStatusCode)
                break;

            if (IsTransientStatus(response.StatusCode) && attempt < maxRetries)
            {
                await Task.Delay(TimeSpan.FromSeconds(attempt * 2), cancellationToken);
                continue;
            }

            break;
        }

        if (response is null)
            return null;

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        if (!response.IsSuccessStatusCode)
            throw CreateFriendlyGoogleBooksException(response.StatusCode);

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
        if (volumeInfo.TryGetProperty("imageLinks", out var imageLinks) &&
            imageLinks.TryGetProperty("thumbnail", out var thumb))
            cover = thumb.GetString() ?? string.Empty;

        var year = 0;
        if (volumeInfo.TryGetProperty("publishedDate", out var pd))
        {
            var dateStr = pd.GetString() ?? string.Empty;
            if (dateStr.Length >= 4 && int.TryParse(dateStr[..4], out var parsedYear))
                year = parsedYear;
        }

        var pages = volumeInfo.TryGetProperty("pageCount", out var pc) ? pc.GetInt32() : 0;
        var previewLink = volumeInfo.TryGetProperty("previewLink", out var pl) ? pl.GetString() : null;

        var genres = new List<string>();
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

    private string BuildVolumesUrl(string query)
    {
        var baseUrl = $"https://www.googleapis.com/books/v1/volumes?q={Uri.EscapeDataString(query.Trim())}&maxResults=20";
        return string.IsNullOrWhiteSpace(_apiKey) ? baseUrl : $"{baseUrl}&key={_apiKey}";
    }

    private string BuildVolumeUrl(string googleBooksId)
    {
        var baseUrl = $"https://www.googleapis.com/books/v1/volumes/{Uri.EscapeDataString(googleBooksId)}";
        return string.IsNullOrWhiteSpace(_apiKey) ? baseUrl : $"{baseUrl}?key={_apiKey}";
    }

    private async Task<string> GetGoogleBooksJsonAsync(string url, CancellationToken cancellationToken)
    {
        HttpResponseMessage? response = null;
        const int maxRetries = 4;

        for (var attempt = 1; attempt <= maxRetries; attempt++)
        {
            response = await _httpClient.GetAsync(url, cancellationToken);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsStringAsync(cancellationToken);

            if (IsTransientStatus(response.StatusCode) && attempt < maxRetries)
            {
                await Task.Delay(TimeSpan.FromSeconds(attempt * 2), cancellationToken);
                continue;
            }

            break;
        }

        throw CreateFriendlyGoogleBooksException(response?.StatusCode ?? HttpStatusCode.ServiceUnavailable);
    }

    private static bool IsTransientStatus(HttpStatusCode statusCode) =>
        statusCode is HttpStatusCode.ServiceUnavailable or (HttpStatusCode)429 or HttpStatusCode.GatewayTimeout;

    private static ExternalServiceException CreateFriendlyGoogleBooksException(HttpStatusCode statusCode)
    {
        var suffix = statusCode switch
        {
            HttpStatusCode.ServiceUnavailable => "Google Books hazırda müvəqqəti olaraq əlçatan deyil (503).",
            (HttpStatusCode)429 => "Google Books sorğu limiti dolub (429).",
            _ => $"Google Books xidmətindən cavab alınmadı ({(int)statusCode})."
        };

        return new ExternalServiceException(
            $"{suffix} Bir neçə dəqiqə sonra yenidən cəhd edin və ya kitabı yuxarıdakı PDF panelindən əl ilə əlavə edin.");
    }
}
