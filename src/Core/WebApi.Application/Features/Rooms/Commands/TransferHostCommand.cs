using MediatR;

namespace WebApi.Application.Features.Rooms.Commands;

public class TransferHostCommand : IRequest
{
    public Guid RoomId { get; set; }
    public string NewHostUserId { get; set; } = string.Empty;
    public string RequestedByUserId { get; set; } = string.Empty;
}