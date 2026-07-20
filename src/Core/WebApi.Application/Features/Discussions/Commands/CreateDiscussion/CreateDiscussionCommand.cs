using MediatR;
using WebApi.Domain.Enums;

namespace WebApi.Application.Features.Discussions.Commands.CreateDiscussion;

public class CreateDiscussionCommand : IRequest<Guid>
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DiscussionCategory Category { get; set; }

    public string AuthorId { get; set; } = string.Empty;
}