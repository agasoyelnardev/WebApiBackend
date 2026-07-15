using MediatR;

namespace WebApi.Application.Features.Social.Commands.SendFriendRequest;

public record SendFriendRequestCommand(string ReceiverId)
    : IRequest<bool>
{
    public string SenderId { get; set; } = string.Empty;
}