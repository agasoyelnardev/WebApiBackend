namespace WebApi.Application.Features.AiChat.Dtos;

public record AskAiChatResponse(
    string Reply,
    List<Guid>? RecommendedMovieIds = null,
    List<Guid>? RecommendedBookIds = null
);