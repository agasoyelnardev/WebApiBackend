using MediatR;
using WebApi.Application.Features.Discussions.Dtos;
using WebApi.Domain.Enums;

namespace WebApi.Application.Features.Discussions.Queries.GetDiscussions;

public class GetDiscussionsQuery : IRequest<List<DiscussionDto>>
{
    public DiscussionCategory? Category { get; set; }
    public string? RequestingUserId { get; set; }

    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}