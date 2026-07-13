using MediatR;

namespace WebApi.Application.Features.Social.Commands.FollowUser;

public record FollowUserCommand(string FollowingUserId) : IRequest<bool>
{
    public string FollowerUserId { get; set; } = string.Empty;
}