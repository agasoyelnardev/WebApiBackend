using MediatR;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Rooms.Commands;

public record DeleteRoomCommand(Guid RoomId) : IRequest<Unit>
{
    public string RequestedByUserId { get; set; } = string.Empty;
}

public class DeleteRoomCommandHandler : IRequestHandler<DeleteRoomCommand, Unit>
{
    private readonly IChatRepository _repository;

    public DeleteRoomCommandHandler(IChatRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await _repository.GetRoomByIdAsync(request.RoomId);
        if (room is null)
            throw new KeyNotFoundException("Otaq tapılmadı");

        if (room.CreatedByUserId != request.RequestedByUserId)
            throw new UnauthorizedAccessException("Bu otağı silmək icazəniz yoxdur");

        await _repository.DeleteRoomAsync(room);
        await _repository.SaveChangesAsync();

        return Unit.Value;
    }
}