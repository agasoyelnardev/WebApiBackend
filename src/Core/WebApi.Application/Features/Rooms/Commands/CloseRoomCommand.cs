using MediatR;
using WebApi.Application.Common.Exceptions;
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

    public CloseRoomCommandHandler(IChatRepository repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(CloseRoomCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.RequestedByUserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        var room = await _repository.GetRoomByIdAsync(request.RoomId);

        if (room is null)
            throw new NotFoundException("Otaq tapılmadı");

        var isAdmin = _currentUserService.IsInRole("Admin");

        if (room.CreatedByUserId != request.RequestedByUserId && !isAdmin)
            throw new UnauthorizedAccessException("Bu otağı bağlamaq icazəniz yoxdur");

        room.IsLive = false;
        await _repository.SaveChangesAsync();

        return Unit.Value;
    }
}