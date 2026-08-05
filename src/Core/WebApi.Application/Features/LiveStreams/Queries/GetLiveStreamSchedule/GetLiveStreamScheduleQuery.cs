using MediatR;
using WebApi.Application.Features.LiveStreams.Dtos;

namespace WebApi.Application.Features.LiveStreams.Queries.GetLiveStreamSchedule;

public class GetLiveStreamScheduleQuery : IRequest<List<LiveStreamScheduleDto>>
{
}