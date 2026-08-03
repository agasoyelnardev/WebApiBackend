using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.Admin.Commands.CloseStreamRoom;

public class CloseStreamRoomCommandHandler : IRequestHandler<CloseStreamRoomCommand, Unit>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CloseStreamRoomCommandHandler(IAppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(CloseStreamRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await _context.StreamRooms
                       .FirstOrDefaultAsync(r => r.Id == request.RoomId && !r.IsDeleted, cancellationToken)
                   ?? throw new NotFoundException("Otaq tapılmadı.");

        room.IsLive = false;
        room.UpdatedAt = DateTime.UtcNow;

        _context.AdminActivityLogs.Add(new AdminActivityLog
        {
            AdminUsername = _currentUserService.Username ?? "Unknown",
            Action = "CLOSE_ROOM",
            Description = "Watch Party otağı bağlandı.",
            TargetEntityType = "StreamRoom",
            TargetEntityId = request.RoomId
        });

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}