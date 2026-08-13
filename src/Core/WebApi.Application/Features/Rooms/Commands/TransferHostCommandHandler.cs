using MediatR;
using Microsoft.AspNetCore.SignalR;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Hubs;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Rooms.Commands;


public class TransferHostCommandHandler : IRequestHandler<TransferHostCommand>
{
    private readonly IChatRepository _repository;
    private readonly IRoomPresenceService _presenceService;
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly ICurrentUserService _currentUserService;  

    public TransferHostCommandHandler(
        IChatRepository repository,
        IRoomPresenceService presenceService,
        IHubContext<ChatHub> hubContext,
        ICurrentUserService currentUserService)
    {
        _repository = repository;
        _presenceService = presenceService;
        _hubContext = hubContext;
        _currentUserService = currentUserService;
    }

    public async Task Handle(TransferHostCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;

        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        var room = await _repository.GetRoomByIdAsync(request.RoomId);

        if (room is null)
            throw new NotFoundException("Otaq tapılmadı.");

        if (room.CreatedByUserId != currentUserId)   
            throw new UnauthorizedAccessException("Yalnız otaq sahibi host statusunu ötürə bilər.");

        if (request.NewHostUserId == currentUserId)  
            throw new BadRequestException("Artıq siz bu otağın sahibisiniz.");

        var isCurrentlyInRoom = _presenceService.IsUserInRoom(request.RoomId.ToString(), request.NewHostUserId);

        if (!isCurrentlyInRoom)
            throw new BadRequestException("Yalnız hazırda otaqda olan istifadəçiyə host statusu verilə bilər.");

        room.CreatedByUserId = request.NewHostUserId;
        await _repository.SaveChangesAsync();

        await _hubContext.Clients.Group(request.RoomId.ToString())
            .SendAsync("HostChanged", new { NewHostUserId = request.NewHostUserId }, cancellationToken);
    }
}