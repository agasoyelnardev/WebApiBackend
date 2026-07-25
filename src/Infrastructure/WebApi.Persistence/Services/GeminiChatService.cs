using System.Net.Http.Json;
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

    public async Task<string> AskGeminiAsync(string userMessage, string? userContextPrompt = null, CancellationToken cancellationToken = default)
    {
        var endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={_apiKey}";

        var systemText = "Sən CineVerse platformasının süni intellekt köməkçisisən (CineAI). " +
                          "İstifadəçilərə kinolar, seriallar və kitablar haqqında dostluq və peşəkar dildə tövsiyələr ver. " +
                          "Cavabları qısa, aydın şəkildə, istifadəçinin müraciət etdiyi dildə ver." +
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
            return "Üzr istəyirəm, hazırda AI servisi ilə əlaqə saxlanıla bilmir. Bir az sonra yenidən cəhd edin.";

        var result = await response.Content.ReadFromJsonAsync<GeminiResponse>(cancellationToken: cancellationToken);
        var reply = result?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text;

        return reply ?? "Təəssüf ki, müvafiq cavab yaradıla bilmədi.";
    }
}