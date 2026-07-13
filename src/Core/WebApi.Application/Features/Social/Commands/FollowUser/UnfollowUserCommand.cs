using MediatR;

namespace WebApi.Application.Features.Social.Commands.UnfollowUser;

public record UnfollowUserCommand(string FollowingUserId) : IRequest<bool>
{
    public string FollowerUserId { get; set; } = string.Empty;
}