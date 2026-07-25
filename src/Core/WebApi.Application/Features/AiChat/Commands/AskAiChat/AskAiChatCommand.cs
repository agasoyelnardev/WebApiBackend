using MediatR;

namespace WebApi.Application.Features.AiChat.Commands.AskAiChat;

public record AskAiChatCommand(string Message, string? UserId) : IRequest<string>;