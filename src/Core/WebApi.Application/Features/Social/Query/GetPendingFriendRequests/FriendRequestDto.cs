namespace WebApi.Application.Features.Social.Queries;

public record FriendRequestDto(
    Guid Id,
    string SenderId,
    string SenderUsername,
    string SenderAvatar,
    DateTime CreatedAt
);