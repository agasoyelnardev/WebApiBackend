using MediatR;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Rooms.Commands;

public record CloseRoomCommand(Guid RoomId) : IRequest<Unit>
{
    public string RequestedByUserId { get; set; } = string.Empty;
}
