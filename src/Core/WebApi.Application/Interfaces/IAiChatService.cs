namespace WebApi.Application.Interfaces;

public interface IAiChatService
{
    Task<string> AskGeminiAsync(string userMessage, string? userContextPrompt = null, CancellationToken cancellationToken = default);
}