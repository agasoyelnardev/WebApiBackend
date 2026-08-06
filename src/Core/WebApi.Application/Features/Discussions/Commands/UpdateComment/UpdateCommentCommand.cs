using MediatR;

namespace WebApi.Application.Features.Discussions.Commands.UpdateComment;

public class UpdateCommentCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
}