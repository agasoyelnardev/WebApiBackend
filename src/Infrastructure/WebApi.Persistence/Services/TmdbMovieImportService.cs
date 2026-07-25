using System.Text.Json;
using Microsoft.Extensions.Configuration;
using WebApi.Application.Interfaces;

namespace WebApi.Persistence.Services;

public class TmdbMovieImportService : IMovieImportService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _baseUrl;

    public TmdbMovieImportService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["ExternalServices:Tmdb:ApiKey"]
                  ?? throw new InvalidOperationException("ExternalServices:Tmdb:ApiKey təyin edilməyib.");
        _baseUrl = configuration["ExternalServices:Tmdb:BaseUrl"]
                   ?? "https://api.themoviedb.org/3/";
    }


    public async Task<List<TmdbMovieSearchResultDto>> SearchAsync(string query, CancellationToken cancellationToken)
    {
        var url = $"{_baseUrl}search/movie?api_key={_apiKey}&query={Uri.EscapeDataString(query)}&language=en-US";

        var response = await _httpClient.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        using var doc = JsonDocument.Parse(json);

        var results = new List<TmdbMovieSearchResultDto>();

        foreach (var item in doc.RootElement.GetProperty("results").EnumerateArray())
        {
            var posterPath = item.TryGetProperty("poster_path", out var posterProp) && posterProp.ValueKind != JsonValueKind.Null
                ? posterProp.GetString()
                : null;

            var releaseDate = item.TryGetProperty("release_date", out var dateProp)
                ? dateProp.GetString()
                : null;

            results.Add(new TmdbMovieSearchResultDto
            {
                TmdbId = item.GetProperty("id").GetInt32(),
                Title = item.GetProperty("title").GetString() ?? string.Empty,
                PosterUrl = posterPath is not null ? $"https://image.tmdb.org/t/p/w500{posterPath}" : null,
                ReleaseYear = !string.IsNullOrEmpty(releaseDate) && releaseDate.Length >= 4 ? releaseDate[..4] : null
            });
        }

        return results;
    }

    public async Task<TmdbMovieDetailDto?> GetDetailsAsync(int tmdbId, CancellationToken cancellationToken)
    {
        var detailsUrl = $"{_baseUrl}movie/{tmdbId}?api_key={_apiKey}&language=en-US&append_to_response=credits,videos";

        var response = await _httpClient.GetAsync(detailsUrl, cancellationToken);
        if (!response.IsSuccessStatusCode)
            return null;

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        var posterPath = root.TryGetProperty("poster_path", out var p) && p.ValueKind != JsonValueKind.Null ? p.GetString() : null;
        var backdropPath = root.TryGetProperty("backdrop_path", out var b) && b.ValueKind != JsonValueKind.Null ? b.GetString() : null;
        var releaseDate = root.TryGetProperty("release_date", out var rd) ? rd.GetString() : null;
        var runtime = root.TryGetProperty("runtime", out var rt) ? rt.GetInt32() : 0;

        var genres = new List<string>();
        if (root.TryGetProperty("genres", out var genresArr))
        {
            foreach (var g in genresArr.EnumerateArray())
                genres.Add(g.GetProperty("name").GetString() ?? string.Empty);
        }

        var cast = new List<string>();
        string director = string.Empty;

        if (root.TryGetProperty("credits", out var credits))
        {
            if (credits.TryGetProperty("cast", out var castArr))
            {
                foreach (var c in castArr.EnumerateArray().Take(10))
                    cast.Add(c.GetProperty("name").GetString() ?? string.Empty);
            }

            if (credits.TryGetProperty("crew", out var crewArr))
            {
                foreach (var c in crewArr.EnumerateArray())
                {
                    if (c.GetProperty("job").GetString() == "Director")
                    {
                        director = c.GetProperty("name").GetString() ?? string.Empty;
                        break;
                    }
                }
            }
        }

        string trailerUrl = string.Empty;
        if (root.TryGetProperty("videos", out var videos) && videos.TryGetProperty("results", out var videoResults))
        {
            foreach (var v in videoResults.EnumerateArray())
            {
                if (v.GetProperty("site").GetString() == "YouTube" && v.GetProperty("type").GetString() == "Trailer")
                {
                    trailerUrl = $"https://www.youtube.com/watch?v={v.GetProperty("key").GetString()}";
                    break;
                }
            }
        }

        return new TmdbMovieDetailDto
        {
            TmdbId = tmdbId,
            Title = root.GetProperty("title").GetString() ?? string.Empty,
            OriginalTitle = root.TryGetProperty("original_title", out var ot) ? ot.GetString() ?? string.Empty : string.Empty,
            Description = root.TryGetProperty("overview", out var ov) ? ov.GetString() ?? string.Empty : string.Empty,
            Poster = posterPath is not null ? $"https://image.tmdb.org/t/p/w500{posterPath}" : string.Empty,
            Banner = backdropPath is not null ? $"https://image.tmdb.org/t/p/original{backdropPath}" : string.Empty,
            Year = !string.IsNullOrEmpty(releaseDate) && releaseDate.Length >= 4 ? int.Parse(releaseDate[..4]) : 0,
            Duration = runtime > 0 ? $"{runtime} dəqiqə" : string.Empty,
            Director = director,
            Genres = genres,
            Cast = cast,
            TrailerUrl = trailerUrl
        };
    }
}