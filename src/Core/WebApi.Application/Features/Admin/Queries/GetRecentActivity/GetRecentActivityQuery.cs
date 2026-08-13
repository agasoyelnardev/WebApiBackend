using MediatR;
using WebApi.Application.Features.Admin.Dtos;

namespace WebApi.Application.Features.Admin.Queries.GetRecentActivity;

public class GetRecentActivityQuery : IRequest<RecentActivityDto>
{
    public int UserCount { get; set; } = 10;
    public int ReviewCount { get; set; } = 10;
}