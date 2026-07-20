using MediatR;

namespace WebApi.Application.Features.Discussions.Commands.CreateComment;

public class CreateCommentCommand : IRequest<Guid>
{
    public Guid DiscussionId { get; set; }
    public string Content { get; set; } = string.Empty;

    public string AuthorId { get; set; } = string.Empty;
}