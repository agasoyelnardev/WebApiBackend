using MediatR;

namespace WebApi.Application.Features.Discussions.Commands.DeleteComment;

public class DeleteCommentCommand : IRequest
{
    public Guid Id { get; set; }
    public string RequestedByUserId { get; set; } = string.Empty;
}