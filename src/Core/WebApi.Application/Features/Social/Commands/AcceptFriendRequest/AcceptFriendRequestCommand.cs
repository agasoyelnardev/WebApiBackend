using MediatR;

namespace WebApi.Application.Features.Social.Commands.AcceptFriendRequest;

public record AcceptFriendRequestCommand(Guid FriendshipId)
    : IRequest<bool>
{
    public string UserId { get; set; } = string.Empty;
}