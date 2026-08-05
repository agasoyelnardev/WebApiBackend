using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Features.LiveStreams.Dtos;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.LiveStreams.Queries.GetActiveLiveStreams;

public class GetActiveLiveStreamsQueryHandler : IRequestHandler<GetActiveLiveStreamsQuery, List<LiveStreamDto>>
{
    private readonly IAppDbContext _context;

    public GetActiveLiveStreamsQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<LiveStreamDto>> Handle(GetActiveLiveStreamsQuery request, CancellationToken cancellationToken)
    {
        return await _context.LiveStreams
            .OrderByDescending(s => s.IsLive)
            .ThenByDescending(s => s.ViewerCount)
            .Select(s => new LiveStreamDto
            {
                Id = s.Id,
                ChannelKey = s.ChannelKey,
                Title = s.Title,
                Description = s.Description,
                StreamUrl = s.StreamUrl,
                ThumbnailUrl = s.ThumbnailUrl,
                IsLive = s.IsLive,
                ViewerCount = s.ViewerCount,
                Category = s.Category,
                StartedAt = s.StartedAt
            })
            .ToListAsync(cancellationToken);
    }
}