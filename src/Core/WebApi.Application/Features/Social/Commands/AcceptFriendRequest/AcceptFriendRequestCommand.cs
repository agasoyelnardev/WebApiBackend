using MediatR;

namespace WebApi.Application.Features.Social.Commands.AcceptFriendRequest;

public record AcceptFriendRequestCommand(Guid FriendshipId) : IRequest<bool> { }