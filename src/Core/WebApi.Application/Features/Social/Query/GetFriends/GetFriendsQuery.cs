using MediatR;

namespace WebApi.Application.Features.Social.Query.GetFriends;

public record GetFriendsQuery(string UserId)
    : IRequest<List<FriendDto>>;