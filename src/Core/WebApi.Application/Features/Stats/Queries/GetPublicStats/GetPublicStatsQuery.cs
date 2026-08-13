using MediatR;

namespace WebApi.Application.Features.Stats.Queries.GetPublicStats;

public record GetPublicStatsQuery : IRequest<PublicStatsDto>;

public class PublicStatsDto
{
    public int OnlineCount { get; set; }
    public int TotalReviews { get; set; }
    public int ActiveRoomsCount { get; set; }
}