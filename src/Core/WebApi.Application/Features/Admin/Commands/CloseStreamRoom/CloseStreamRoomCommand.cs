using MediatR;

namespace WebApi.Application.Features.Admin.Commands.CloseStreamRoom;

public class CloseStreamRoomCommand : IRequest<Unit>
{
    public Guid RoomId { get; set; }
}