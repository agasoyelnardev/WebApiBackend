namespace WebApi.Application.Features.Social.Query.GetFriends;

public record FriendDto(
    string Id,
    string UserName,
    string Avatar
);