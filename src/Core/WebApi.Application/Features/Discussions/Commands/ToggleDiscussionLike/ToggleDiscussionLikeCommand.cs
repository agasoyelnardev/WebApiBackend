using MediatR;

namespace WebApi.Application.Features.Discussions.Commands.ToggleDiscussionLike;

public class ToggleDiscussionLikeCommand : IRequest<bool>
{
    public Guid DiscussionId { get; set; }
    public string UserId { get; set; } = string.Empty;
}