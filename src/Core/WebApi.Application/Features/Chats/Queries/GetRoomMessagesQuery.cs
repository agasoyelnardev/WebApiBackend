using MediatR;

namespace WebApi.Application.Features.Chats.Queries;

public record GetRoomMessagesQuery(Guid RoomId) : IRequest<List<ChatMessageDto>>;