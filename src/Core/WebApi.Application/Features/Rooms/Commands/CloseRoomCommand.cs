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
    private readonly ICurrentUserService _currentUserService;

    public CloseRoomCommandHandler(IChatRepository repository,ICurrentUserService _currentUserService)
    {
        _repository = repository;
        this._currentUserService = _currentUserService;
    }

    public async Task<Unit> Handle(CloseRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await _repository.GetRoomByIdAsync(request.RoomId);
        var isAdmin = _currentUserService.IsInRole("Admin");

        if (room.CreatedByUserId != request.RequestedByUserId && !isAdmin)
        {
            throw new UnauthorizedAccessException(
                "Bu otağı bağlamaq icazəniz yoxdur");
        }
        if (room is null)
            throw new KeyNotFoundException("Otaq tapılmadı");

        room.IsLive = false;
        await _repository.SaveChangesAsync();

        return Unit.Value;
    }
}