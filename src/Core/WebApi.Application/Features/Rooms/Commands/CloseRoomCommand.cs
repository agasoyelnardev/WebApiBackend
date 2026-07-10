using MediatR;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Rooms.Commands;

public record CloseRoomCommand(Guid RoomId) : IRequest<Unit>
{
    public string RequestedByUserId { get; set; } = string.Empty;
}

public class CloseRoomCommandHandler : IRequestHandler<CloseRoomCommand, Unit>
{
    private readonly IChatRepository _repository;

    public CloseRoomCommandHandler(IChatRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(CloseRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await _repository.GetRoomByIdAsync(request.RoomId);
        if (room is null)
            throw new KeyNotFoundException("Otaq tapılmadı");

        if (room.CreatedByUserId != request.RequestedByUserId)
            throw new UnauthorizedAccessException("Bu otağı bağlamaq icazəniz yoxdur");

        room.IsLive = false;
        await _repository.SaveChangesAsync();

        return Unit.Value;
    }
}