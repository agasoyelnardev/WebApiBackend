namespace WebApi.Application.Features.Social.Queries.GetPendingFriendRequests;

public record FriendRequestDto(
    Guid Id,
    string SenderId,
    string SenderUsername,
    string SenderAvatar,
    DateTime CreatedAt
);