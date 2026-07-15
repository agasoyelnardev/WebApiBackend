using MediatR;

namespace WebApi.Application.Features.Social.Commands.RemoveFriend;

public record RemoveFriendCommand(string FriendUserId)
    : IRequest<bool>
{
    public string CurrentUserId { get; set; } = string.Empty;
}