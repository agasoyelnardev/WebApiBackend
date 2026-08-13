using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Rooms.Commands.InviteToRoom;

public class InviteToRoomCommandHandler : IRequestHandler<InviteToRoomCommand>
{
    private readonly IAppDbContext _context;
    private readonly IChatRepository _chatRepository;
    private readonly INotificationService _notificationService;

    public InviteToRoomCommandHandler(
        IAppDbContext context,
        IChatRepository chatRepository,
        INotificationService notificationService)
    {
        _context = context;
        _chatRepository = chatRepository;
        _notificationService = notificationService;
    }

    public async Task Handle(InviteToRoomCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.SenderUserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        if (request.RecipientUserId == request.SenderUserId)
            throw new BadRequestException("Özünüzü otağa dəvət edə bilməzsiniz.");

        var room = await _chatRepository.GetRoomByIdAsync(request.RoomId);

        if (room is null)
            throw new NotFoundException("Otaq tapılmadı.");

        if (!room.IsLive)
            throw new BadRequestException("Bu otaq artıq bağlıdır.");

        var recipientExists = await _context.Users
            .AnyAsync(u => u.Id == request.RecipientUserId, cancellationToken);

        if (!recipientExists)
            throw new NotFoundException("Dəvət edilən istifadəçi tapılmadı.");
        

        var sender = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.SenderUserId, cancellationToken);

        await _notificationService.NotifyAsync(
            userId: request.RecipientUserId,
            type: "party_invite",
            title: "Watch Party Dəvəti 🎬",
            description: $"{sender?.UserName ?? "Bir istifadəçi"} sizi \"{room.Title}\" otağına dəvət etdi.",
            relatedEntityId: room.Id,
            cancellationToken: cancellationToken);
    }
}