using MediatR;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Chats.Queries;

public record GetRoomMessagesQuery(Guid RoomId) : IRequest<List<ChatMessageDto>>;

public class GetRoomMessagesQueryHandler
    : IRequestHandler<GetRoomMessagesQuery, List<ChatMessageDto>>
{
    private readonly IChatRepository _repository;

    public GetRoomMessagesQueryHandler(IChatRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ChatMessageDto>> Handle(
        GetRoomMessagesQuery request, CancellationToken cancellationToken)
    {
        var room = await _repository.GetRoomByIdAsync(request.RoomId);
        if (room is null)
            throw new KeyNotFoundException("Otaq tapılmadı");

        var messages = await _repository.GetMessagesByRoomIdAsync(request.RoomId);

        return messages.Select(m => new ChatMessageDto(
            m.Id,
            m.UserId,
            m.Username,
            m.UserAvatarUrl,
            m.MessageText,
            m.IsSystemMessage,
            m.CreatedAt 
        )).ToList();
    }
}