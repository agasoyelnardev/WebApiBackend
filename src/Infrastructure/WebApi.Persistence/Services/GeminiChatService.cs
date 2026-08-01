using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using WebApi.Application.Dtos.External.Gemini;
using WebApi.Application.Interfaces;

namespace WebApi.Persistence.Services;

public class GeminiChatService : IAiChatService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public GeminiChatService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["Gemini:ApiKey"]
            ?? throw new InvalidOperationException("Gemini:ApiKey təyin edilməyib.");
    }

    public async Task<AiChatResult> AskGeminiAsync(string userMessage, string? userContextPrompt = null, CancellationToken cancellationToken = default)
    {
        var endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={_apiKey}";

        var systemText = "Sən CineVerse platformasının süni intellekt köməkçisisən (CineAI). " +
                          "İstifadəçilərə kinolar, seriallar və kitablar haqqında dostluq və peşəkar dildə tövsiyələr ver. " +
                          "Cavabları qısa, aydın şəkildə, istifadəçinin müraciət etdiyi dildə ver. " +
                          "CAVABINI HƏMİŞƏ YALNIZ aşağıdakı JSON formatında ver, başqa heç nə əlavə etmə, markdown ```json işarələri qoyma: " +
                          "{\"reply\": \"cavab mətni\", \"recommendedMovies\": [\"Film Adı 1\", \"Film Adı 2\"], \"recommendedBooks\": [\"Kitab Adı 1\"]}. " +
                          "Əgər tövsiyə ediləcək konkret film/kitab yoxdursa, həmin massivləri boş ([]) burax. " +
                          "recommendedMovies və recommendedBooks-a maksimum 3 element daxil et, yalnız CineVerse platformasında mövcud ola biləcək real, tanınmış film/kitab adları yaz." +
                          (string.IsNullOrEmpty(userContextPrompt) ? "" : " " + userContextPrompt);

        var request = new GeminiRequest
        {
            SystemInstruction = new SystemInstruction
            {
                Parts = new List<GeminiPart> { new() { Text = systemText } }
            },
            Contents = new List<GeminiContent>
            {
                new()
                {
                    Role = "user",
                    Parts = new List<GeminiPart> { new() { Text = userMessage } }
                }
            }
        };

        var response = await _httpClient.PostAsJsonAsync(endpoint, request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return new AiChatResult
            {
                Reply = "Üzr istəyirəm, hazırda AI servisi ilə əlaqə saxlanıla bilmir. Bir az sonra yenidən cəhd edin."
            };
        }

        var result = await response.Content.ReadFromJsonAsync<GeminiResponse>(cancellationToken: cancellationToken);
        var rawText = result?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text;

        if (string.IsNullOrWhiteSpace(rawText))
        {
            return new AiChatResult
            {
                Reply = "Təəssüf ki, müvafiq cavab yaradıla bilmədi."
            };
        }

        return ParseStructuredReply(rawText);
    }

    private static AiChatResult ParseStructuredReply(string rawText)
    {
        // Gemini bəzən ```json ... ``` şəklində markdown fence əlavə edə bilər — təmizləyirik
        var cleaned = rawText.Trim();
        if (cleaned.StartsWith("```"))
        {
            var firstNewline = cleaned.IndexOf('\n');
            var lastFence = cleaned.LastIndexOf("```");
            if (firstNewline > -1 && lastFence > firstNewline)
                cleaned = cleaned[(firstNewline + 1)..lastFence].Trim();
        }

        try
        {
            using var doc = JsonDocument.Parse(cleaned);
            var root = doc.RootElement;

            var reply = root.TryGetProperty("reply", out var replyProp)
                ? replyProp.GetString() ?? string.Empty
                : rawText;

            var movies = new List<string>();
            if (root.TryGetProperty("recommendedMovies", out var moviesProp) && moviesProp.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in moviesProp.EnumerateArray())
                {
                    var title = item.GetString();
                    if (!string.IsNullOrWhiteSpace(title))
                        movies.Add(title);
                }
            }

            var books = new List<string>();
            if (root.TryGetProperty("recommendedBooks", out var booksProp) && booksProp.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in booksProp.EnumerateArray())
                {
                    var title = item.GetString();
                    if (!string.IsNullOrWhiteSpace(title))
                        books.Add(title);
                }
            }

            return new AiChatResult
            {
                Reply = reply,
                RecommendedMovieTitles = movies.Take(3).ToList(),
                RecommendedBookTitles = books.Take(3).ToList()
            };
        }
        catch (JsonException)
        {
            // Gemini JSON formatına əməl etməyibsə, bütün mətni sadəcə cavab kimi qaytarırıq
            return new AiChatResult { Reply = rawText };
        }
    }
}