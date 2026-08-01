using MediatR;

namespace WebApi.Application.Features.Social.Commands.DeclineFriendRequest;

public record DeclineFriendRequestCommand(Guid FriendshipId) : IRequest<bool>;