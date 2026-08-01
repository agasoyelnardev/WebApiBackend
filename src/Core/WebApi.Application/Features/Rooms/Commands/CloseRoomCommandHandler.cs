using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Rooms.Commands;

public class CloseRoomCommandHandler : IRequestHandler<CloseRoomCommand, Unit>
{
    private readonly IChatRepository _repository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAppDbContext _context;
    private readonly INotificationService _notificationService;

    public CloseRoomCommandHandler(
        IChatRepository repository,
        ICurrentUserService currentUserService,
        IAppDbContext context,
        INotificationService notificationService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
        _context = context;
        _notificationService = notificationService;
    }

    public async Task<Unit> Handle(CloseRoomCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;

        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        var room = await _repository.GetRoomByIdAsync(request.RoomId);

        if (room is null)
            throw new NotFoundException("Otaq tapılmadı");

        var isAdmin = _currentUserService.IsInRole("Admin");

        if (room.CreatedByUserId != currentUserId && !isAdmin)   
            throw new UnauthorizedAccessException("Bu otağı bağlamaq icazəniz yoxdur");

        room.IsLive = false;
        await _repository.SaveChangesAsync();

        var participantIds = await _context.ChatMessages
            .Where(m => m.StreamRoomId == request.RoomId && m.UserId != currentUserId)   
            .Select(m => m.UserId)
            .Distinct()
            .ToListAsync(cancellationToken);

        foreach (var userId in participantIds)
        {
            await _notificationService.NotifyAsync(
                userId: userId,
                type: "room_closed",
                title: "Otaq bağlandı",
                description: $"\"{room.Title}\" otağı bağlandı.",
                relatedEntityId: room.Id,
                cancellationToken: cancellationToken);
        }

        return Unit.Value;
    }
}