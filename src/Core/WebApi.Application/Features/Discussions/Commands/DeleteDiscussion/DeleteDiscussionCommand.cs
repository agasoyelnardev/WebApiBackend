using MediatR;

namespace WebApi.Application.Features.Discussions.Commands.DeleteDiscussion;

public class DeleteDiscussionCommand : IRequest
{
    public Guid Id { get; set; }
    public string RequestedByUserId { get; set; } = string.Empty;
}