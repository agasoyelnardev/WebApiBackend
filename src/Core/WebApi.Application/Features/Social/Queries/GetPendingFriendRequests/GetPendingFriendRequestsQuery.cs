using MediatR;

namespace WebApi.Application.Features.Social.Queries.GetPendingFriendRequests;

public record GetPendingFriendRequestsQuery(string UserId)
    : IRequest<List<FriendRequestDto>>;