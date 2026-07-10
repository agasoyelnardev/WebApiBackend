namespace WebApi.Application.Features.Chats.Queries;

public record ChatMessageDto(
    Guid Id,
    string UserId,
    string Username,
    string UserAvatarUrl,
    string MessageText,
    bool IsSystemMessage,
    DateTime CreatedAt
);