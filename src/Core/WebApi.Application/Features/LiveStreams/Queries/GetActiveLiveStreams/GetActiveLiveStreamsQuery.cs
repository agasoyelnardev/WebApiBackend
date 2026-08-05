using MediatR;
using WebApi.Application.Features.LiveStreams.Dtos;

namespace WebApi.Application.Features.LiveStreams.Queries.GetActiveLiveStreams;

public class GetActiveLiveStreamsQuery : IRequest<List<LiveStreamDto>>
{
}