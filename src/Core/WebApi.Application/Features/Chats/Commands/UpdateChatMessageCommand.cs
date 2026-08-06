using MediatR;

namespace WebApi.Application.Features.Chats.Commands;

public class UpdateChatMessageCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public string MessageText { get; set; } = string.Empty;
}