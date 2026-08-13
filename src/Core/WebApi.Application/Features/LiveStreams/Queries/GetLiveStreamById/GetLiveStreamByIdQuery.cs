using MediatR;
using WebApi.Application.Features.LiveStreams.Dtos;

namespace WebApi.Application.Features.LiveStreams.Queries.GetLiveStreamById;

public record GetLiveStreamByIdQuery(Guid Id) : IRequest<LiveStreamDto?>;