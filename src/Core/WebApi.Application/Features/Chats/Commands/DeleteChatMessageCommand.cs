using MediatR;

namespace WebApi.Application.Features.Chats.Commands;

public record DeleteChatMessageCommand(Guid Id) : IRequest;