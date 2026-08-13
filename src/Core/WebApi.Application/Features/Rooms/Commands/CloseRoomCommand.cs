using MediatR;

namespace WebApi.Application.Features.Rooms.Commands;

public record CloseRoomCommand(Guid RoomId) : IRequest<Unit>
{
}
