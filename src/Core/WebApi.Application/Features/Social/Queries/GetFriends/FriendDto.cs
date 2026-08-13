namespace WebApi.Application.Features.Social.Queries.GetFriends;

public record FriendDto(
    string Id,
    string UserName,
    string Avatar
);