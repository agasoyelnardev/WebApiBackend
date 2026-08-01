using MediatR;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Rooms.Commands;

public record DeleteRoomCommand(Guid RoomId) : IRequest<Unit>
{
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
        var currentUserId = _currentUserService.UserId;

        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        var room = await _repository.GetRoomByIdAsync(request.RoomId);

        if (room is null)
            throw new NotFoundException("Otaq tapılmadı");

        var isAdmin = _currentUserService.IsInRole("Admin");

        if (room.CreatedByUserId != currentUserId && !isAdmin)   
            throw new UnauthorizedAccessException("Bu otağı silmək icazəniz yoxdur");

        await _repository.DeleteRoomAsync(room);
        await _repository.SaveChangesAsync();

        return Unit.Value;
    }
}