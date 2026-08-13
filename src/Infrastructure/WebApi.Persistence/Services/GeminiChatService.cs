using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebApi.Application.Dtos.External.Gemini;
using WebApi.Application.Interfaces;

namespace WebApi.Persistence.Services;

public class GeminiChatService : IAiChatService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<GeminiChatService> _logger;
    private readonly string? _apiKey;
    private readonly string[] _models;

    public GeminiChatService(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<GeminiChatService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _apiKey = ResolveApiKey(configuration);

        var configuredModel = configuration["Gemini:Model"];
        _models = new[]
            {
                configuredModel,
                "gemini-flash-latest",
                "gemini-3.6-flash",
                "gemini-3.5-flash-lite",
                "gemini-2.5-flash",
            }
            .Where(model => !string.IsNullOrWhiteSpace(model))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Cast<string>()
            .ToArray();
    }

    public async Task<AiChatResult> AskGeminiAsync(
        string userMessage,
        string? userContextPrompt = null,
        CancellationToken cancellationToken = default)
    {
        if (!IsConfiguredApiKey(_apiKey))
        {
            return new AiChatResult
            {
                Reply =
                    "CineAI hazırda aktiv deyil. Backend-də Gemini API açarı təyin edilməyib. " +
                    "Admin `Gemini:ApiKey` (appsettings) və ya `GEMINI_API_KEY` environment variable əlavə etməlidir."
            };
        }

        var systemText = BuildSystemPrompt(userContextPrompt);
        var request = BuildRequest(userMessage, systemText);
        var sawRateLimit = false;

        foreach (var model in _models)
        {
            var endpoint =
                $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={_apiKey}";

            var response = await RequestWithRateLimitRetryAsync(endpoint, request, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogWarning(
                    "Gemini API xətası ({StatusCode}) model={Model}: {ErrorBody}",
                    (int)response.StatusCode,
                    model,
                    Truncate(errorBody, 400));

                if ((int)response.StatusCode is 400 or 404 or 429)
                {
                    if ((int)response.StatusCode == 429)
                        sawRateLimit = true;
                    continue;
                }

                return new AiChatResult
                {
                    Reply = MapGeminiFailureMessage((int)response.StatusCode)
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

        return new AiChatResult
        {
            Reply = sawRateLimit
                ? "CineAI sorğu limitinə çatdı. Bir neçə dəqiqə sonra yenidən cəhd edin."
                : "CineAI hazırda cavab verə bilmir. Gemini API açarını və model konfiqurasiyasını yoxlayın."
        };
    }

    private static string? ResolveApiKey(IConfiguration configuration)
    {
        return FirstNonEmpty(
            configuration["Gemini:ApiKey"],
            configuration["GEMINI_API_KEY"],
            Environment.GetEnvironmentVariable("GEMINI_API_KEY"),
            Environment.GetEnvironmentVariable("Gemini__ApiKey"));
    }

    private static bool IsConfiguredApiKey(string? apiKey)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
            return false;

        var normalized = apiKey.Trim();
        return normalized is not (
            "MY_GEMINI_API_KEY"
            or "your_key"
            or "YOUR_GEMINI_API_KEY"
            or "your-gemini-api-key"
        ) && !normalized.StartsWith("YOUR_", StringComparison.OrdinalIgnoreCase);
    }

    private static string BuildSystemPrompt(string? userContextPrompt)
    {
        var systemText =
            "Sən CineVerse platformasının süni intellekt köməkçisisən (CineAI). " +
            "İstifadəçilərə kinolar, seriallar və kitablar haqqında dostluq və peşəkar dildə tövsiyələr ver. " +
            "Cavabları qısa, aydın şəkildə, istifadəçinin müraciət etdiyi dildə ver. " +
            "CAVABINI HƏMİŞƏ YALNIZ aşağıdakı JSON formatında ver, başqa heç nə əlavə etmə, markdown ```json işarələri qoyma: " +
            "{\"reply\": \"cavab mətni\", \"recommendedMovies\": [\"Film Adı 1\", \"Film Adı 2\"], \"recommendedBooks\": [\"Kitab Adı 1\"]}. " +
            "Əgər tövsiyə ediləcək konkret film/kitab yoxdursa, həmin massivləri boş ([]) burax. " +
            "recommendedMovies və recommendedBooks-a maksimum 3 element daxil et, yalnız CineVerse platformasında mövcud ola biləcək real, tanınmış film/kitab adları yaz.";

        return string.IsNullOrEmpty(userContextPrompt)
            ? systemText
            : $"{systemText} {userContextPrompt}";
    }

    private static GeminiRequest BuildRequest(string userMessage, string systemText) => new()
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

    private static string MapGeminiFailureMessage(int statusCode) => statusCode switch
    {
        401 or 403 =>
            "CineAI API açarı etibarsızdır. Zəhmət olmasa Gemini API key-i yeniləyin.",
        429 =>
            "CineAI sorğu limitinə çatdı. Bir neçə dəqiqə sonra yenidən cəhd edin.",
        _ =>
            "Üzr istəyirəm, hazırda AI servisi ilə əlaqə saxlanıla bilmir. Bir az sonra yenidən cəhd edin."
    };

    private async Task<HttpResponseMessage> RequestWithRateLimitRetryAsync(
        string endpoint,
        GeminiRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync(endpoint, request, cancellationToken);
        if ((int)response.StatusCode != 429)
            return response;

        await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
        return await _httpClient.PostAsJsonAsync(endpoint, request, cancellationToken);
    }

    private static string? FirstNonEmpty(params string?[] values) =>
        values.FirstOrDefault(value => !string.IsNullOrWhiteSpace(value))?.Trim();

    private static string Truncate(string value, int maxLength) =>
        value.Length <= maxLength ? value : value[..maxLength] + "...";

    private static AiChatResult ParseStructuredReply(string rawText)
    {
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
            return new AiChatResult { Reply = rawText };
        }
    }
}
