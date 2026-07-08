using MediatR;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.Chats.Queries;

public record GetMessagesQuery(Guid StreamRoomId) : IRequest<List<ChatMessage>>;

public class GetMessagesQueryHandler : IRequestHandler<GetMessagesQuery, List<ChatMessage>>
{
    private readonly IChatRepository _chatRepository;

    public GetMessagesQueryHandler(IChatRepository chatRepository)
    {
        _chatRepository = chatRepository;
    }

    public async Task<List<ChatMessage>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
    {
        return await _chatRepository.GetMessagesByRoomIdAsync(request.StreamRoomId);
    }
}