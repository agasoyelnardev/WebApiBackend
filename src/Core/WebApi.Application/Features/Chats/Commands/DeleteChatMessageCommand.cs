using MediatR;

namespace WebApi.Application.Features.Chats.Commands;

public class DeleteChatMessageCommand : IRequest
{
    public Guid Id { get; set; }

    public DeleteChatMessageCommand(Guid id)
    {
        Id = id;
    }
}