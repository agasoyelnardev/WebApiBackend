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
    private readonly ICurrentUserService _currentUserService;

    public DeleteRoomCommandHandler(IChatRepository repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await _repository.GetRoomByIdAsync(request.RoomId);
        var isAdmin = _currentUserService.IsInRole("Admin");

        if (room.CreatedByUserId != request.RequestedByUserId && !isAdmin)
        {
            throw new UnauthorizedAccessException(
                "Bu otağı silmək icazəniz yoxdur");
        }
        
        if (room is null)
            throw new KeyNotFoundException("Otaq tapılmadı");
        

        await _repository.DeleteRoomAsync(room);
        await _repository.SaveChangesAsync();

        return Unit.Value;
    }
}