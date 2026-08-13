namespace WebApi.Application.Interfaces;

public class AiChatResult
{
    public string Reply { get; set; } = string.Empty;
    public List<string> RecommendedMovieTitles { get; set; } = new();
    public List<string> RecommendedBookTitles { get; set; } = new();
}

public interface IAiChatService
{
    Task<AiChatResult> AskGeminiAsync(string userMessage, string? userContextPrompt = null, CancellationToken cancellationToken = default);
}