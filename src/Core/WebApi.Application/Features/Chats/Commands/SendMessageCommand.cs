using MediatR;

namespace WebApi.Application.Features.Chats.Commands;

public record SendMessageCommand(Guid RoomId, string MessageText) : IRequest<Guid>
{
    public string UserId { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string UserAvatarUrl { get; set; } = string.Empty;
}