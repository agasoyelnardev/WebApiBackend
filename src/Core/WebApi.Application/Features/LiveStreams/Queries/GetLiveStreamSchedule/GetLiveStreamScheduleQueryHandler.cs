using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Features.LiveStreams.Dtos;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.LiveStreams.Queries.GetLiveStreamSchedule;

public class GetLiveStreamScheduleQueryHandler : IRequestHandler<GetLiveStreamScheduleQuery, List<LiveStreamScheduleDto>>
{
    private readonly IAppDbContext _context;

    public GetLiveStreamScheduleQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<LiveStreamScheduleDto>> Handle(GetLiveStreamScheduleQuery request, CancellationToken cancellationToken)
    {
        return await _context.LiveStreamSchedules
            .Where(s => s.AirTime >= DateTime.UtcNow)
            .OrderBy(s => s.AirTime)
            .Select(s => new LiveStreamScheduleDto
            {
                Id = s.Id,
                ChannelKey = s.ChannelKey,
                ProgramTitle = s.ProgramTitle,
                Description = s.Description,
                AirTime = s.AirTime,
                DurationMinutes = s.DurationMinutes
            })
            .ToListAsync(cancellationToken);
    }
}