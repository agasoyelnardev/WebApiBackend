using MediatR;
using WebApi.Application.Features.AiChat.Dtos;

namespace WebApi.Application.Features.AiChat.Commands.AskAiChat;

public record AskAiChatCommand(string Message) : IRequest<AskAiChatResponse>;