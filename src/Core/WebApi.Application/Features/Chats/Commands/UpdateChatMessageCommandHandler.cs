// WebApi.Application/Features/Chats/Commands/UpdateChatMessageCommandHandler.cs
using MediatR;
using Microsoft.AspNetCore.SignalR;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Hubs;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Chats.Commands;

public class UpdateChatMessageCommandHandler : IRequestHandler<UpdateChatMessageCommand, Unit>
{
    private readonly IChatRepository _repository;
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly ICurrentUserService _currentUserService;

    public UpdateChatMessageCommandHandler(
        IChatRepository repository,
        IHubContext<ChatHub> hubContext,
        ICurrentUserService currentUserService)
    {
        _repository = repository;
        _hubContext = hubContext;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(UpdateChatMessageCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;

        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        if (string.IsNullOrWhiteSpace(request.MessageText))
            throw new BadRequestException("Mesaj boş ola bilməz.");

        if (request.MessageText.Length > 500)
            throw new BadRequestException("Mesaj maksimum 500 simvol ola bilər.");

        var message = await _repository.GetMessageByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Mesaj tapılmadı.");

        var isAdmin = _currentUserService.IsInRole("Admin");

        if (message.UserId != currentUserId && !isAdmin)
            throw new UnauthorizedAccessException("Bu mesajı redaktə etmək hüququnuz yoxdur.");

        if (message.IsSystemMessage)
            throw new BadRequestException("Sistem mesajları redaktə edilə bilməz.");

        message.MessageText = request.MessageText;
        message.UpdatedAt = DateTime.UtcNow;

        await _repository.SaveChangesAsync(cancellationToken);

        await _hubContext.Clients.Group(message.StreamRoomId.ToString())
            .SendAsync("MessageUpdated", new
            {
                message.Id,
                message.MessageText
            }, cancellationToken);

        return Unit.Value;
    }
}