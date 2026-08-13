using MediatR;
using WebApi.Application.Features.Discussions.Dtos;

namespace WebApi.Application.Features.Discussions.Queries.GetDiscussionById;

public class GetDiscussionByIdQuery : IRequest<DiscussionDetailDto?>
{
    public Guid Id { get; set; }
    public string? RequestingUserId { get; set; }

    public GetDiscussionByIdQuery(Guid id, string? requestingUserId)
    {
        Id = id;
        RequestingUserId = requestingUserId;
    }
}