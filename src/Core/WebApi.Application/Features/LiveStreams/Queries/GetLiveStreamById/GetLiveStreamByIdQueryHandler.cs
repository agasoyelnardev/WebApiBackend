using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Features.LiveStreams.Dtos;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.LiveStreams.Queries.GetLiveStreamById;

public class GetLiveStreamByIdQueryHandler : IRequestHandler<GetLiveStreamByIdQuery, LiveStreamDto?>
{
    private readonly IAppDbContext _context;

    public GetLiveStreamByIdQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<LiveStreamDto?> Handle(GetLiveStreamByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.LiveStreams
            .Where(s => s.Id == request.Id)
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
            .FirstOrDefaultAsync(cancellationToken);
    }
}