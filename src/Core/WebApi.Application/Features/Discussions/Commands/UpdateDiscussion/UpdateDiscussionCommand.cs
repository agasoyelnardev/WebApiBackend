using MediatR;
using WebApi.Domain.Enums;

namespace WebApi.Application.Features.Discussions.Commands.UpdateDiscussion;

public class UpdateDiscussionCommand : IRequest
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DiscussionCategory Category { get; set; }

    public string RequestedByUserId { get; set; } = string.Empty;
}