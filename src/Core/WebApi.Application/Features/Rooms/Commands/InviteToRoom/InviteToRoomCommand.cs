using MediatR;

namespace WebApi.Application.Features.Rooms.Commands.InviteToRoom;

public class InviteToRoomCommand : IRequest
{
    public Guid RoomId { get; set; }
    public string RecipientUserId { get; set; } = string.Empty;
    public string SenderUserId { get; set; } = string.Empty;
}