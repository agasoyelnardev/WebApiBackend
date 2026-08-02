using MediatR;
using WebApi.Application.Features.Social.Queries.GetFriends;

namespace WebApi.Application.Features.Social.Query.GetFriends;

public record GetFriendsQuery(string UserId)
    : IRequest<List<FriendDto>>;